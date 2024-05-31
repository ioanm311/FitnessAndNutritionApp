using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessAndNutritionApp.Models
{
    public class FitnessPlanDetail
    {
        public FitnessPlanDetail()
        {
            Exercises = new List<Exercise>();  
        }
        [Key]
        public int FitnessDetailID { get; set; }
        public int FitnessPlanID { get; set; }
        public string Day { get; set; }

        [ForeignKey("FitnessPlanID")]
        public FitnessPlan FitnessPlan { get; set; }

        public List<Exercise> Exercises { get; set; }
    }
}
