namespace WorkoutPlanner.Models
{
    public class WorkoutPlan
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public DateTime Date { get; set; } 
        public string? UserId { get; set; }
        public List<Exercise> Exercises { get; set; } = new List<Exercise>();
        public int? RegimenId { get; set; } // Foreign key
        public Regimen Regimen { get; set; }
    }
    public class UpdateDateModel
    {
        public int Id { get; set; }
        public DateTime NewDate { get; set; }
    }
}
