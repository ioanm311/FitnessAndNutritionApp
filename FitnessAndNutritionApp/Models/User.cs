using Microsoft.AspNetCore.Identity;

namespace FitnessAndNutritionApp.Models
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
