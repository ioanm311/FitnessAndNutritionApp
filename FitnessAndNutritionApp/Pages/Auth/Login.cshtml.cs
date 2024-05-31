using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FitnessAndNutritionApp.Pages.Auth
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager; 

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager; 
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(Input.Email, Input.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(Input.Email);
                    var roles = await _userManager.GetRolesAsync(user);

                    var isAdmin = roles.Any(role => role == "Admin");

                    HttpContext.Session.SetString("UserRole", isAdmin ? "Admin" : "User");

                    if (isAdmin)
                    {
                        return RedirectToPage("/Admin/AdminPage");
                    }
                    else
                    {
                        return LocalRedirect(Url.Content("~/"));
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "The email or password provided is incorrect.");
                    return Page();
                }
            }

            return Page();
        }
    }
}
