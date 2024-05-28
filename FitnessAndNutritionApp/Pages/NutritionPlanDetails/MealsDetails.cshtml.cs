using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Pages.NutritionPlanDetails
{
    public class MealsDetailsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public MealsDetailsModel(ApplicationDbContext context, UserManager<User> userManager) : base(userManager)
        {
            _context = context;
        }

        [BindProperty]
        public Meal Meal { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Meal = await _context.Meals.FindAsync(id);

            if (Meal == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
