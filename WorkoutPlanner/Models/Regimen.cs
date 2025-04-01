using System.ComponentModel.DataAnnotations;

namespace WorkoutPlanner.Models
{
    public class Regimen
    {
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(500)]
        public string Description { get; set; }
        public string? CreatedByUserId { get; set; } // Link to creator
        public List<RegimenExercise> Exercises { get; set; } = new();
    }
}
