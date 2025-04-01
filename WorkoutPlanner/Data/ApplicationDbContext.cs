using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Models;


namespace WorkoutPlanner.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<ExerciseTemplate> ExerciseTemplates { get; set; }
        public DbSet<Regimen> Regimens { get; set; }
        public DbSet<RegimenExercise> RegimenExercises { get; set; }
        public DbSet<WorkoutProgress> WorkoutProgress { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExerciseTemplate>().HasData(
                new ExerciseTemplate
                {
                    Id = 1,
                    Name = "Squats",
                    MuscleGroup = "Legs",
                    Type = "Legs",
                    Description = "A lower body exercise targeting quads, glutes, and hamstrings.",
                    Equipment = "Barbell",
                    Difficulty = 2,
                    VideoUrl = "https://www.youtube.com/watch?v=Dh4BtSpExA0"
                },
                new ExerciseTemplate
                {
                    Id = 2,
                    Name = "Push-ups",
                    MuscleGroup = "Chest",
                    Type = "Push",
                    Description = "An upper body exercise for chest, shoulders, and triceps.",
                    Equipment = "Bodyweight",
                    Difficulty = 1,
                    VideoUrl = "https://www.youtube.com/watch?v=IODxDxX7oi4"
                },
                new ExerciseTemplate
                {
                    Id = 3,
                    Name = "Deadlifts",
                    MuscleGroup = "Back",
                    Type = "Pull",
                    Description = "A full-body lift focusing on back, glutes, and hamstrings.",
                    Equipment = "Barbell",
                    Difficulty = 3,
                    VideoUrl = "https://www.youtube.com/watch?v=ytGaGIn3SjE"
                },
                new ExerciseTemplate
                {
                    Id = 4,
                    Name = "Bench Press",
                    MuscleGroup = "Chest",
                    Type = "Push",
                    Description = "A chest exercise using a barbell or dumbbells.",
                    Equipment = "Barbell",
                    Difficulty = 3,
                    VideoUrl = "https://www.youtube.com/watch?v=rT7DgCr-3_E"
                },
                new ExerciseTemplate
                {
                    Id = 5,
                    Name = "Pull-ups",
                    MuscleGroup = "Back",
                    Type = "Pull",
                    Description = "An upper body exercise targeting lats and biceps.",
                    Equipment = "Pull-Up Bar",
                    Difficulty = 4,
                    VideoUrl = "https://www.youtube.com/watch?v=eGo4IYlbE5g"
                }
            );
            modelBuilder.Entity<RegimenExercise>()
        .HasKey(re => new { re.RegimenId, re.ExerciseTemplateId });

            modelBuilder.Entity<Regimen>().HasData(
                new Regimen { Id = 1, Name = "Beginner Full Body", Description = "A simple full-body workout.", CreatedByUserId = "seed-user" }
            );

            modelBuilder.Entity<RegimenExercise>().HasData(
                new RegimenExercise { RegimenId = 1, ExerciseTemplateId = 2, Sets = 3, Reps = 10 }, // Push-ups
                new RegimenExercise { RegimenId = 1, ExerciseTemplateId = 1, Sets = 3, Reps = 12 }  // Squats
            );
        }
    }
}
