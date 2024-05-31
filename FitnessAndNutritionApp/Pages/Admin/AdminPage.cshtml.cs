using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FitnessAndNutritionApp.Pages.Admin
{
    public class AdminPageModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public AdminPageModel(UserManager<User> userManager, ApplicationDbContext context) : base(userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public string SearchTerm { get; set; }
        public List<User> Users { get; set; }
        public List<User> AllUsers { get; set; }
        public IDictionary<string, int> PlanUsageStats { get; set; }
        public IDictionary<int, int> ExerciseFrequencyStats { get; set; }
        public IDictionary<string, int> NutritionPlanUsageStats { get; set; }
        public IDictionary<int, int> MealFrequencyStats { get; set; }

        public void OnGet()
        {
            LoadStatistics();
        }

        public async Task<IActionResult> OnPostSearchAsync()
        {
            if (!string.IsNullOrEmpty(SearchTerm))
            {
                Users = await _userManager.Users
                    .Where(u => u.FirstName.Contains(SearchTerm) || u.LastName.Contains(SearchTerm) || u.UserName.Contains(SearchTerm))
                    .ToListAsync();
            }
            LoadStatistics();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
            LoadStatistics();
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostGetAllUsersAsync()
        {
            AllUsers = await _userManager.Users.ToListAsync();
            LoadStatistics();
            return Page();
        }

        private void LoadStatistics()
        {
            // Statisticile de utilizare a planurilor de fitness
            PlanUsageStats = _context.FitnessPlans
                .GroupBy(p => p.PlanType)
                .Select(g => new { PlanType = g.Key, Count = g.Count() })
                .ToDictionary(x => x.PlanType, x => x.Count);

            // Frecventa exercitiilor
            ExerciseFrequencyStats = _context.FitnessPlanDetails
                .SelectMany(fpd => fpd.Exercises)
                .GroupBy(e => e.ExerciseID)
                .Select(g => new { ExerciseID = g.Key, Count = g.Count() })
                .ToDictionary(x => x.ExerciseID, x => x.Count);

            // Statisticile de utilizare a planurilor de nutritie
            NutritionPlanUsageStats = _context.NutritionPlans
                .GroupBy(p => p.PlanType)
                .Select(g => new { PlanType = g.Key, Count = g.Count() })
                .ToDictionary(x => x.PlanType, x => x.Count);

            // Frecventa meselor
            MealFrequencyStats = _context.NutritionPlanDetails
                .SelectMany(npd => npd.Meals)
                .GroupBy(m => m.MealID)
                .Select(g => new { MealID = g.Key, Count = g.Count() })
                .ToDictionary(x => x.MealID, x => x.Count);
        }
    }
}
