using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessAndNutritionApp.Pages.Nutrition
{
    public class ChoosePlanModel : BasePageModel  // Inherits from BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public ChoosePlanModel(ApplicationDbContext context, UserManager<User> userManager)
            : base(userManager)  // Call constructor from BasePageModel
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/LoginPromptError/LoginPrompt");
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string planChoice)
        {
            var userId = _userManager.GetUserId(User);
            if (!string.IsNullOrEmpty(userId))
            {
                var existingPlans = await _context.NutritionPlans
                                          .Include(np => np.NutritionPlanDetails)
                                          .ThenInclude(npd => npd.Meals)
                                          .Where(np => np.UserID == int.Parse(userId))
                                          .ToListAsync();

                foreach (var plan in existingPlans)
                {
                    plan.IsActive = false;  // Deactivate all existing plans
                }

                // Check if there's already a plan of the same type
                var nutritionPlan = existingPlans.FirstOrDefault(np => np.PlanType == planChoice);
                if (nutritionPlan == null)
                {
                    // Create a new plan if no reusable plan exists
                    nutritionPlan = new NutritionPlan
                    {
                        UserID = int.Parse(userId),
                        PlanType = planChoice,
                        IsActive = true,
                        LastResetDate = DateTime.Now
                    };

                    _context.NutritionPlans.Add(nutritionPlan);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    nutritionPlan.IsActive = true;  // Reactivate the existing plan
                    nutritionPlan.LastResetDate = DateTime.Now;
                    _context.Update(nutritionPlan);
                    await _context.SaveChangesAsync();
                }

                // Handle the details for each day of the week
                var days = new List<string> { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                foreach (var day in days)
                {
                    var planDetail = await _context.NutritionPlanDetails
                                                   .FirstOrDefaultAsync(p => p.Day == day && p.NutritionPlanID == nutritionPlan.NutritionPlanID);

                    if (planDetail == null)
                    {
                        planDetail = new NutritionPlanDetail
                        {
                            NutritionPlanID = nutritionPlan.NutritionPlanID,
                            Day = day,
                            Meals = new List<Meal>()
                        };
                        _context.NutritionPlanDetails.Add(planDetail);
                        await _context.SaveChangesAsync();
                    }

                    // Load meals for the current day and plan
                    var mealsForDay = await GetMealsForPlanAndDay(planChoice, day);

                    // Add meals if not already associated
                    if (!planDetail.Meals.Any())
                    {
                        foreach (var meal in mealsForDay)
                        {
                            if (!planDetail.Meals.Contains(meal))
                            {
                                planDetail.Meals.Add(meal);
                            }
                        }
                        await _context.SaveChangesAsync();
                    }
                }

                // Final save to ensure all changes are committed
                await _context.SaveChangesAsync();
                return RedirectToPage("/NutritionPlanDetails/PlanDetails");
            }

            return Page();
        }
        private async Task<List<Meal>> GetMealsForPlanAndDay(string planType, string dayOfWeek)
        {
            return await _context.Meals
                                 .Where(m => m.PlanType == planType && m.DayOfWeek == dayOfWeek)
                                 .ToListAsync();
        }
    }
}
