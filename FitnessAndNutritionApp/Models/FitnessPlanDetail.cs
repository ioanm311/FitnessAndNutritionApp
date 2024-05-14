using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessAndNutritionApp.Models
{
    public class FitnessPlanDetail
    {
        public FitnessPlanDetail()
        {
            Exercises = new List<Exercise>();  // Inițializează lista pentru a evita null
        }
        [Key]
        public int FitnessDetailID { get; set; }
        public int FitnessPlanID { get; set; }
        public string Day { get; set; }

        // Proprietate de navigație către FitnessPlan
        [ForeignKey("FitnessPlanID")]
        public FitnessPlan FitnessPlan { get; set; }

        // Relație many-to-many cu Exercitiu
        public List<Exercise> Exercises { get; set; }
    }
}
