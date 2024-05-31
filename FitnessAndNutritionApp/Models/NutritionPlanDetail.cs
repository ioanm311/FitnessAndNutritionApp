using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FitnessAndNutritionApp.Models
{
    public class NutritionPlanDetail
    {
        public NutritionPlanDetail()
        {
            Meals = new List<Meal>();  
        }

        [Key]
        public int NutritionDetailID { get; set; }
        public int NutritionPlanID { get; set; }
        public string Day { get; set; }

        [ForeignKey("NutritionPlanID")]
        public NutritionPlan NutritionPlan { get; set; }

        public List<Meal> Meals { get; set; }
    }
}
