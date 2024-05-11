using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;

namespace FitnessAndNutritionApp.Pages.Company
{
    public class AboutUsModel : BasePageModel
    {
        public AboutUsModel(UserManager<User> userManager) : base(userManager)
        {
        }
        public void OnGet()
        {
        }
    }
}
