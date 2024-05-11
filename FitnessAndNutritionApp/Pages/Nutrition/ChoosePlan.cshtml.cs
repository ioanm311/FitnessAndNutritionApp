using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessAndNutritionApp.Pages.Nutrition
{
    [Authorize]
    public class ChoosePlanModel : BasePageModel  // Inherits from BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public ChoosePlanModel(ApplicationDbContext context, UserManager<User> userManager)
            : base(userManager)  // Call constructor from BasePageModel
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(string planChoice)
        {
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                var existingPlans = await _context.NutritionPlans
                                          .Where(np => np.UserID == int.Parse(userId))
                                          .ToListAsync();

                foreach (var plan in existingPlans)
                {
                    plan.IsActive = false;  // Deactivate all existing plans
                }

                // Check if there's already a plan of the same type
                var reusablePlan = existingPlans.FirstOrDefault(np => np.PlanType == planChoice);
                if (reusablePlan != null)
                {
                    reusablePlan.IsActive = true;  // Reactivate the existing plan
                    reusablePlan.LastResetDate = DateTime.Now;
                }
                else
                {
                    // Create a new plan if no reusable plan exists
                    var nutritionPlan = new NutritionPlan
                    {
                        UserID = int.Parse(userId),
                        PlanType = planChoice,
                        IsActive = true,
                        LastResetDate = DateTime.Now
                    };

                    _context.NutritionPlans.Add(nutritionPlan);
                }

                await _context.SaveChangesAsync();
                return RedirectToPage("/NutritionPlanDetails/PlanDetails");
            }

            return Page();
        }
    }
}
