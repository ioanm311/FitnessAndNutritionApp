using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Models
{
    public class BasePageModel : PageModel
    {
        private readonly UserManager<User> _userManager;

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
                    // Verifică dacă utilizatorul este admin
                    var isAdmin = await _userManager.IsInRoleAsync(user, "Admin");

                    // Setează numele pentru utilizator în funcție de rol
                    ViewData["DisplayName"] = isAdmin ? user.LastName : user.FirstName;
                }
            }

            // Nu uita să apelezi metoda next pentru a continua execuția lanțului de Page Handlers.
            await next();
        }
    }
}
