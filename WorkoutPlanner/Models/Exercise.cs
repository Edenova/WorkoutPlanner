using System.ComponentModel.DataAnnotations;

namespace WorkoutPlanner.Models
{
    public class Exercise
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; } // e.g., "Barbell Bench Press"

        [Required]
        [StringLength(50)]
        public string MuscleGroup { get; set; } // e.g., "Chest", "Legs", "Back"

        [StringLength(50)]
        public string Type { get; set; } // e.g., "Push", "Pull", "Legs", "Core"

        [StringLength(500)]
        public string Description { get; set; } // e.g., "A compound lift targeting the pectorals..."

        [StringLength(50)]
        public string Equipment { get; set; } // e.g., "Barbell", "Dumbbells", "Bodyweight"

        [Range(1, 5)]
        public int Difficulty { get; set; } // 1 (Beginner) to 5 (Advanced)

        public string VideoUrl { get; set; } // e.g., YouTube link for demo
        public int Sets { get; set; }
        public int Reps { get; set; }
        public int WorkoutPlanId { get; set; } // Foreign key
        public WorkoutPlan WorkoutPlan { get; set; } // Navigation property
    }
}
