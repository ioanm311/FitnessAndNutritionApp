using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;

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
                // Deactivate all existing plans
                var existingPlans = await _context.FitnessPlans
                                                  .Include(fp => fp.FitnessPlanDetails)
                                                  .ThenInclude(fpd => fpd.Exercises)
                                                  .Where(fp => fp.UserID == int.Parse(userId))
                                                  .ToListAsync();

                foreach (var plan in existingPlans)
                {
                    plan.IsActive = false;
                }

                // Find or create the fitness plan
                var fitnessPlan = existingPlans.FirstOrDefault(fp => fp.PlanType == planChoice);
                if (fitnessPlan == null)
                {
                    fitnessPlan = new FitnessPlan
                    {
                        UserID = int.Parse(userId),
                        PlanType = planChoice,
                        IsActive = true,
                        LastResetDate = DateTime.Now
                    };
                    _context.FitnessPlans.Add(fitnessPlan);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    fitnessPlan.IsActive = true;
                    fitnessPlan.LastResetDate = DateTime.Now;
                    _context.Update(fitnessPlan);
                    await _context.SaveChangesAsync();
                }

                // Handle the details for each day
                var days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
                foreach (var day in days)
                {
                    var planDetail = await _context.FitnessPlanDetails
                                                   .FirstOrDefaultAsync(p => p.Day == day && p.FitnessPlanID == fitnessPlan.FitnessPlanID);

                    if (planDetail == null)
                    {
                        planDetail = new FitnessPlanDetail
                        {
                            FitnessPlanID = fitnessPlan.FitnessPlanID,
                            Day = day,
                            Exercises = new List<Exercise>()
                        };
                        _context.FitnessPlanDetails.Add(planDetail);
                        await _context.SaveChangesAsync();
                    }

                    // Load exercises for the current day and plan
                    var exercisesForDay = await GetExercisesForPlanAndDay(planChoice, day);

                    // Add exercises if not already associated
                    if (!planDetail.Exercises.Any())
                    {
                        foreach (var exercise in exercisesForDay)
                        {
                            if (!planDetail.Exercises.Contains(exercise))
                            {
                                planDetail.Exercises.Add(exercise);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                // Final save to ensure all changes are committed
                await _context.SaveChangesAsync();
                return RedirectToPage("/FitnessPlanDetails/PlanDetails");
            }

            return Page();
        }

        private async Task<List<Exercise>> GetExercisesForPlanAndDay(string planType, string dayOfWeek)
        {
            return await _context.Exercises
                                 .Where(e => e.PlanType == planType && e.DayOfWeek == dayOfWeek)
                                 .ToListAsync();
        }

    }

}
