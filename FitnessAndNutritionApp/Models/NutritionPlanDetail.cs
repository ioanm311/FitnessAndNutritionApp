using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessAndNutritionApp.Models
{
    public class NutritionPlanDetail
    {
        public NutritionPlanDetail()
        {
            Meals = new List<Meal>();  // Inițializează lista pentru a evita null
        }

        [Key]
        public int NutritionDetailID { get; set; }
        public int NutritionPlanID { get; set; }
        public string Day { get; set; }

        // Proprietate de navigație către NutritionPlan
        [ForeignKey("NutritionPlanID")]
        public NutritionPlan NutritionPlan { get; set; }

        // Relație many-to-many cu Meal
        public List<Meal> Meals { get; set; }
    }
}
