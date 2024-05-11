using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Pages.NutritionPlanDetails
{
    public class PlanDetailsModel : BasePageModel
    {
        public PlanDetailsModel(UserManager<User> userManager) : base(userManager)
        {
        }
        public void OnGet()
        {
        }
    }
}
