namespace WorkoutPlanner.Models
{
    public class PlanRegimenViewModel
    {
        public int RegimenId { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(6);
        public List<string> SelectedDays { get; set; } = new List<string>();
        public bool PreviewOnly { get; set; } = false;
    }
}