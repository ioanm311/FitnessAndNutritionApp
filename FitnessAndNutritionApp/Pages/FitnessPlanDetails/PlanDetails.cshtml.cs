using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

namespace FitnessAndNutritionApp.Pages.FitnessPlanDetails
{
    public class PlanDetailsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context; 
        private readonly UserManager<User> _userManager;

        public PlanDetailsModel(ApplicationDbContext context, UserManager<User> userManager) : base(userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedDay { get; set; } 

        public List<Exercise> ExercisesForDay { get; set; } 

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SelectedDay))
            {
                var userId = _userManager.GetUserId(User);
                var activePlanTypes = _context.FitnessPlans
                                              .Where(fp => fp.UserID.ToString() == userId && fp.IsActive)
                                              .Select(fp => fp.PlanType)
                                              .Distinct()
                                              .ToList();  

                if (activePlanTypes.Any())
                {
                    ExercisesForDay = _context.Exercises
                                              .Where(e => e.DayOfWeek == SelectedDay && activePlanTypes.Contains(e.PlanType))
                                              .ToList();
                }
            }
        }
    }
}
