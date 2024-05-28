using System.ComponentModel.DataAnnotations;

namespace FitnessAndNutritionApp.Models
{
    public class CalorieCalculatorModel
    {
        [Required]
        public string Gender { get; set; }

        [Required]
        [Range(10, 150)]
        public int Weight { get; set; }  // in kilograms

        [Required]
        [Range(50, 250)]
        public int Height { get; set; }  // in centimeters

        [Required]
        [Range(18, 100)]
        public int Age { get; set; }

        [Required]
        public string ActivityLevel { get; set; }

        public double BMR { get; set; }
        public double DailyCalorieNeeds { get; set; }
    }
}
