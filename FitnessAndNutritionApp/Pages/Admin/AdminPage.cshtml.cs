using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Pages.Admin
{
    public class AdminPageModel : BasePageModel
    {
        public AdminPageModel(UserManager<User> userManager) : base(userManager)
        {
        }
        public void OnGet()
        {
        }
    }
}
