using FitnessAndNutritionApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FitnessAndNutritionApp.DAL
{
    public class ApplicationDbContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, IdentityUserRole<int>, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<FitnessPlan> FitnessPlans { get; set; }
        public DbSet<NutritionPlan> NutritionPlans { get; set; }
        public DbSet<FitnessPlanDetail> FitnessPlanDetails { get; set; }
        public DbSet<NutritionPlanDetail> NutritionPlanDetails { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<Meal> Meals { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare relație many-to-many între FitnessPlanDetail și Exercise
            modelBuilder.Entity<FitnessPlanDetail>()
            .HasMany(fpd => fpd.Exercises)
            .WithMany(e => e.FitnessPlanDetails)
            .UsingEntity<Dictionary<string, object>>(
                "FitnessPlanDetailExercise", // Numele tabelului de join
                j => j.HasOne<Exercise>()
                    .WithMany()
                    .HasForeignKey("ExerciseID"), // Nume personalizat pentru FK către Exercise
                j => j.HasOne<FitnessPlanDetail>()
                    .WithMany()
                    .HasForeignKey("FitnessDetailID") // Nume personalizat pentru FK către FitnessPlanDetail
            );

            // Configurare relație many-to-many între NutritionPlanDetail și Meal
            modelBuilder.Entity<NutritionPlanDetail>()
                .HasMany(npd => npd.Meals)
                .WithMany(m => m.NutritionPlanDetails)
                .UsingEntity<Dictionary<string, object>>(
                    "NutritionPlanDetailMeal", // Numele tabelului de join
                    j => j.HasOne<Meal>()
                        .WithMany()
                        .HasForeignKey("MealID"), // Nume personalizat pentru FK către Meal
                    j => j.HasOne<NutritionPlanDetail>()
                        .WithMany()
                        .HasForeignKey("NutritionDetailID") // Nume personalizat pentru FK către NutritionPlanDetail
                );
        }
    }
}
