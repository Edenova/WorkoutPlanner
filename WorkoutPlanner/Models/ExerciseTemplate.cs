using System.ComponentModel.DataAnnotations;

namespace WorkoutPlanner.Models
{
    public class ExerciseTemplate
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [Required]
        [StringLength(50)]
        public string MuscleGroup { get; set; }
        [StringLength(50)]
        public string Type { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        [StringLength(50)]
        public string Equipment { get; set; }
        [Range(1, 5)]
        public int Difficulty { get; set; }
        public string VideoUrl { get; set; }
    }
}