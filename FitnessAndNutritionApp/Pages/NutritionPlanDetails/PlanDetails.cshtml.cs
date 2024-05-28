using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Linq;

namespace FitnessAndNutritionApp.Pages.NutritionPlanDetails
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
        public string SelectedDay { get; set; } // Ziua selectată din frontend

        public List<Meal> MealsForDay { get; set; } // Lista de mese pentru ziua selectată

        public void OnGet()
        {
            if (!string.IsNullOrEmpty(SelectedDay))
            {
                var userId = _userManager.GetUserId(User);
                var activePlanTypes = _context.NutritionPlans
                                              .Where(np => np.UserID.ToString() == userId && np.IsActive)
                                              .Select(np => np.PlanType)
                                              .Distinct()
                                              .ToList();  // Colectează tipurile de planuri active

                if (activePlanTypes.Any())
                {
                    MealsForDay = _context.Meals
                                          .Where(m => m.DayOfWeek == SelectedDay && activePlanTypes.Contains(m.PlanType))
                                          .ToList();
                }
            }
        }
    }
}
