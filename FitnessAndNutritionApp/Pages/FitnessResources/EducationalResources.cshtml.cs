using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Pages.FitnessResources
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
    }
}
