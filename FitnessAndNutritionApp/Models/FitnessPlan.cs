using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessAndNutritionApp.Models
{
    public class FitnessPlan
    {
        [Key]
        public int FitnessPlanID { get; set; } 

        [ForeignKey("User")]
        public int UserID { get; set; }  
        public User User { get; set; } 

        public string PlanType { get; set; } 

        public bool IsActive { get; set; }

        [DataType(DataType.Date)]
        public DateTime LastResetDate { get; set; }

        public List<FitnessPlanDetail> FitnessPlanDetails { get; set; }

    }
}
