using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessAndNutritionApp.DAL;
using FitnessAndNutritionApp.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FitnessAndNutritionApp.Pages.FitnessPlanDetails
{
    public class ExerciseDetailsModel : BasePageModel
    {
        private readonly ApplicationDbContext _context;

        public ExerciseDetailsModel(ApplicationDbContext context, UserManager<User> userManager) : base(userManager)
        {
            _context = context;
        }

        [BindProperty]
        public Exercise Exercise { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Exercise = await _context.Exercises.FindAsync(id);

            if (Exercise == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
