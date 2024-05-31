using FitnessAndNutritionApp.Models;
using FitnessAndNutritionApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace FitnessAndNutritionApp.Pages.Auth
{
    public class ResetPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;

        public ResetPasswordModel(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public string Email { get; set; }

        [BindProperty]
        [Required]
        [DataType(DataType.Password)]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*\W).{6,}$", ErrorMessage = "Password must be at least 6 characters long and contain at least one uppercase letter, one digit, and one special character.")]
        public string Password { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }

        [BindProperty]
        public string Token { get; set; }

        public IActionResult OnGet(string userId, string token, string email)
        {
            Email = email;
            Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            if (string.IsNullOrEmpty(Token) || string.IsNullOrEmpty(Email))
            {
                return RedirectToPage("/Index");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "An error has occurred.");
                return Page();
            }
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(Token));

            var result = await ManualResetPasswordAsync(user, decodedToken, Password);
            if (result.Succeeded)
            {
                return RedirectToPage("./ResetPasswordConfirmation");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }

        private async Task<IdentityResult> ManualResetPasswordAsync(User user, string token, string newPassword)
        {
            // Aici se verifica token-ul si parola manual
            if (!await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token))
            {
                return IdentityResult.Failed(new IdentityError { Description = "Invalid token." });
            }

            if (newPassword != ConfirmPassword)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Password and confirmation password do not match." });
            }

            user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, newPassword);
            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                return updateResult;
            }

            return IdentityResult.Success;
        }
    }
}
