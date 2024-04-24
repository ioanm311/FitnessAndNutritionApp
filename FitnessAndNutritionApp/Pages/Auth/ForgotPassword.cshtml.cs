using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using FitnessAndNutritionApp.Models;
using FitnessAndNutritionApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FitnessAndNutritionApp.Pages.Auth
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly IEmailSender _emailSender;

        public ForgotPasswordModel(
            UserManager<User> userManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        [BindProperty]
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (String.IsNullOrEmpty(Email)) // Verifică dacă câmpul de email este gol
            {
                ModelState.AddModelError(string.Empty, "Please enter your email address."); // Adaugă mesaj de eroare
                return Page();
            }

            var user = await _userManager.FindByEmailAsync(Email);
            if (user != null)
            {
                // Generează tokenul folosind UserManager în loc de TokenService
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token)); // Encode token-ul pentru URL

                // Crează URL-ul de callback pentru resetarea parolei
                var callbackUrl = Url.Page(
                    "/Auth/ResetPassword",
                    pageHandler: null,
                    values: new { userId = user.Id, token = encodedToken, email = user.Email },
                    protocol: Request.Scheme);

                // Trimite email-ul cu link-ul de resetare a parolei
                await _emailSender.SendEmailAsync(
                    Email,
                    "Reset Password",
                    $"Please reset your password by clicking here: <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>link</a>");

                // Redirecționează utilizatorul la pagina de confirmare
                return RedirectToPage("./ForgotPasswordConfirmation");
            }
            // Dacă nu s-a găsit utilizatorul sau email-ul nu este confirmat, pur și simplu redirecționează la pagina de confirmare
            // fără să dezvălui această informație pentru securitatea utilizatorului
            return RedirectToPage("./ForgotPasswordConfirmation");
        }
    }
}
