namespace FitnessAndNutritionApp.Models
{
    public class Exercise
    {
        public int ExerciseID { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public string ImageURL { get; set; }
        public string VideoURL { get; set; } 
        public string PlanType { get; set; }
        public string DayOfWeek { get; set; }

        public List<FitnessPlanDetail> FitnessPlanDetails { get; set; }
    }
}
