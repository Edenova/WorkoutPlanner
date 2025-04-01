namespace WorkoutPlanner.Models
{
    public class WorkoutProgress
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int WorkoutPlanId { get; set; }
        public DateTime CompletedDate { get; set; }
        public WorkoutPlan WorkoutPlan { get; set; }
    }
}
