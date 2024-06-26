﻿using FitnessAndNutritionApp.Models;
using FitnessAndNutritionApp.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FitnessAndNutritionApp.Pages.Auth
{
    public class SignUpModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public SignUpModel(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [BindProperty]
        public SignUpViewModel SignUpInfo { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var existingUser = await _userManager.FindByEmailAsync(SignUpInfo.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("SignUpInfo.Email", "A user with this email already exists.");
                return Page();
            }

            if (SignUpInfo.Password != SignUpInfo.ConfirmPassword)
            {
                ModelState.AddModelError(string.Empty, "Passwords do not match.");
                return Page();
            }

            var user = new User
            {
                FirstName = SignUpInfo.FirstName,
                LastName = SignUpInfo.LastName,
                Email = SignUpInfo.Email,
                UserName = SignUpInfo.Email, 
            };

            // Creează utilizatorul si hash-ul parolei
            var result = await _userManager.CreateAsync(user, SignUpInfo.Password);

            if (result.Succeeded)
            {
                // Atribuie utilizatorului rolul de "User"
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded)
                {
                    // Gestioneaza cazul in care atribuirea rolului nu reuseste
                    foreach (var error in roleResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return Page();
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToPage("/Index");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return Page();
        }
    }
}
