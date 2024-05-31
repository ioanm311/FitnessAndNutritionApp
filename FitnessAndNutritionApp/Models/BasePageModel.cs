using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Models
{
    public class BasePageModel : PageModel
    {
        protected readonly UserManager<User> _userManager;

        public BasePageModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public override async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                    // Seteaza numele pentru utilizator in functie de rol
                    ViewData["DisplayName"] = isAdmin ? user.LastName : user.FirstName;
                }
            }
            await next();
        }
    }
}
