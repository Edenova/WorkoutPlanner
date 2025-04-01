namespace WorkoutPlanner.Models
{
    public class RegimenExercise
    {
        public int Id { get; set; }
        public int RegimenId { get; set; }
        public Regimen Regimen { get; set; }
        public int ExerciseTemplateId { get; set; }
        public ExerciseTemplate ExerciseTemplate { get; set; }
        public int Sets { get; set; }
        public int Reps { get; set; }
    }
}
