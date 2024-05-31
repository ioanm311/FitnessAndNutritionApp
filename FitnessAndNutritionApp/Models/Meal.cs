namespace FitnessAndNutritionApp.Models
{
    public class Meal
    {
        public int MealID { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
        public string Preparation { get; set; }
        public string ImageURL { get; set; } 
        public string PlanType { get; set; }
        public string DayOfWeek { get; set; }

        public List<NutritionPlanDetail> NutritionPlanDetails { get; set; }
    }
}
