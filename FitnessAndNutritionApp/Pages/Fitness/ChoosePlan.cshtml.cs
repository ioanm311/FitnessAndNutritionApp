using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FitnessAndNutritionApp.Pages.Fitness
{
    [Authorize]
    public class ChoosePlanModel : BasePageModel  // Moștenirea de la BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public ChoosePlanModel(ApplicationDbContext context, UserManager<User> userManager)
            : base(userManager)  // Apelează constructorul din BasePageModel
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(string planChoice)
        {
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                var existingPlans = await _context.FitnessPlans
                                          .Where(fp => fp.UserID == int.Parse(userId))
                                          .ToListAsync();

                foreach (var plan in existingPlans)
                {
                    plan.IsActive = false;  // Dezactivează toate planurile
                }

                // Verifică dacă există deja un plan de același tip
                var reusablePlan = existingPlans.FirstOrDefault(fp => fp.PlanType == planChoice);
                if (reusablePlan != null)
                {
                    reusablePlan.IsActive = true;  // Reactivează planul existent
                    reusablePlan.LastResetDate = DateTime.Now;
                }
                else
                {
                    // Creează un nou plan dacă nu există unul reutilizabil
                    var fitnessPlan = new FitnessPlan
                    {
                        UserID = int.Parse(userId),
                        PlanType = planChoice,
                        IsActive = true,
                        LastResetDate = DateTime.Now
                    };

                    _context.FitnessPlans.Add(fitnessPlan);
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("/FitnessPlanDetails/PlanDetails");
            }

            return Page();
        }
    }
}
