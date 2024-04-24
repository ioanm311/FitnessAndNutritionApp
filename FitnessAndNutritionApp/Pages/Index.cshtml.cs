using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Pages
{
    public class IndexModel : BasePageModel
    {
        public IndexModel(UserManager<User> userManager) : base(userManager)
        {
        }

        public void OnGet()
        {

        }
    }
}
