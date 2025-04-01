namespace WorkoutPlanner.Models
{
    public class WorkoutPlanCreateViewModel
    {
        public string Title { get; set; }
        public DateTime Date { get; set; }
        public int? RegimenId { get; set; }
    }
}