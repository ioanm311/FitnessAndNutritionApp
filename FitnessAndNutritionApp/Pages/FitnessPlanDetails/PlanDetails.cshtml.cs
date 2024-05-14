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
        private readonly ApplicationDbContext _context; // Asigură-te că numele contextului este corect
        private readonly UserManager<User> _userManager;

        public PlanDetailsModel(ApplicationDbContext context, UserManager<User> userManager) : base(userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string SelectedDay { get; set; } // Ziua selectată din frontend

        public List<Exercise> ExercisesForDay { get; set; } // Lista de exerciții pentru ziua selectată

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SelectedDay))
            {
                var userId = _userManager.GetUserId(User);
                var activePlanTypes = _context.FitnessPlans
                                              .Where(fp => fp.UserID.ToString() == userId && fp.IsActive)
                                              .Select(fp => fp.PlanType)
                                              .Distinct()
                                              .ToList();  // Colectează tipurile de planuri active

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
