using System.ComponentModel.DataAnnotations;

namespace WorkoutPlanner.Models
{
    public class RegimenCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public List<RegimenExerciseInput> Exercises { get; set; } = new();

        public class RegimenExerciseInput
        {
            public int ExerciseTemplateId { get; set; }
            public int Sets { get; set; }
            public int Reps { get; set; }
        }
    }
}
