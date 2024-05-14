namespace FitnessAndNutritionApp.Models
{
    public class Exercise
    {
        public int ExerciseID { get; set; } // Identificator unic pentru fiecare exercițiu
        public string Name { get; set; } // Numele exercițiului
        public string Description { get; set; } // Descriere detaliată a exercițiului
        public string ImageURL { get; set; } // URL-ul către imaginea asociată exercițiului
        public string VideoURL { get; set; } // URL-ul către videoclipul demonstrativ al exercițiului
        public string PlanType { get; set; }
        public string DayOfWeek { get; set; }

        // Relație many-to-many cu FitnessPlanDetail
        public List<FitnessPlanDetail> FitnessPlanDetails { get; set; }
    }
}
