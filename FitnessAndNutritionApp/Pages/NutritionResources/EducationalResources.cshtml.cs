using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnessAndNutritionApp.Pages.NutritionResources
{
    public class EducationalResourcesModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        public EducationalResourcesModel(ApplicationDbContext context, UserManager<User> userManager)
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

        [BindProperty]
        public CalorieCalculatorModel CalorieCalculator { get; set; }

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                CalorieCalculator.BMR = CalculateBMR(CalorieCalculator);
                CalorieCalculator.DailyCalorieNeeds = CalculateDailyCalorieNeeds(CalorieCalculator);
            }
        }

        private double CalculateBMR(CalorieCalculatorModel model)
        {
            if (model.Gender == "Male")
            {
                return 10 * model.Weight + 6.25 * model.Height - 5 * model.Age + 5;
            }
            else
            {
                return 10 * model.Weight + 6.25 * model.Height - 5 * model.Age - 161;
            }
        }

        private double CalculateDailyCalorieNeeds(CalorieCalculatorModel model)
        {
            double activityFactor = model.ActivityLevel switch
            {
                "Sedentary" => 1.2,
                "LightlyActive" => 1.375,
                "ModeratelyActive" => 1.55,
                "VeryActive" => 1.725,
                "SuperActive" => 1.9,
                _ => 1.2
            };
            return model.BMR * activityFactor;
        }
    }
}
