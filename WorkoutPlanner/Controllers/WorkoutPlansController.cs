using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkoutPlanner.Data;
using WorkoutPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace WorkoutPlanner.Controllers
{
    public class WorkoutPlansController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public WorkoutPlansController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: WorkoutPlans
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            
            if (userId == null)
            {
                return View(new List<WorkoutPlan>());
            }
            var workoutPlans = await _context.WorkoutPlans
                .Include(w => w.Regimen) 
                .Where(w => w.UserId == userId)
                .ToListAsync();
            return View(workoutPlans);
        }

        // GET: WorkoutPlans/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            

            var plan = await _context.WorkoutPlans
                .Include(w => w.Regimen)
                .ThenInclude(r => r.Exercises) // Assuming Regimen has ICollection<RegimenExercise> Exercises
                .ThenInclude(re => re.ExerciseTemplate)
                .Include(w => w.Exercises)
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);

            if (plan == null)
            {
                
                return NotFound();
            }

            // If Exercises empty, populate from Regimen
            if (!plan.Exercises.Any() && plan.Regimen?.Exercises != null)
            {
                plan.Exercises = plan.Regimen.Exercises.Select(re => new Exercise
                {
                    Name = re.ExerciseTemplate.Name,
                    MuscleGroup = re.ExerciseTemplate.MuscleGroup,
                    Type = re.ExerciseTemplate.Type,
                    Description = re.ExerciseTemplate.Description,
                    Equipment = re.ExerciseTemplate.Equipment,
                    Difficulty = re.ExerciseTemplate.Difficulty,
                    VideoUrl = re.ExerciseTemplate.VideoUrl,
                    Sets = re.Sets,
                    Reps = re.Reps,
                    WorkoutPlanId = plan.Id
                }).ToList();
                
            }

            return View(plan);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(int workoutPlanId, int exerciseTemplateId, int sets, int reps)
        {
            var workoutPlan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(w => w.Id == workoutPlanId && w.UserId == _userManager.GetUserId(User));
            if (workoutPlan == null)
            {
                return NotFound();
            }
            var template = await _context.ExerciseTemplates.FindAsync(exerciseTemplateId);
            if (template == null)
            {
                return NotFound();
            }
            var exercise = new Exercise
            {
                Name = template.Name,
                Sets = sets,
                Reps = reps,
                WorkoutPlanId = workoutPlanId,
                MuscleGroup = template.MuscleGroup ?? "Unknown",
                Type = template.Type ?? "General",
                Description = template.Description ?? "",
                Equipment = template.Equipment ?? "None",
                Difficulty = template.Difficulty > 0 ? template.Difficulty : 1,
                VideoUrl = template.VideoUrl
            };
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = workoutPlanId });
        }

        [Authorize]
        public async Task<IActionResult> Calendar()
        {
            var userId = _userManager.GetUserId(User);
            var workoutPlans = await _context.WorkoutPlans
                .Where(w => w.UserId == userId)
                .Include(w => w.Regimen)
                .ThenInclude(r => r.Exercises)
                .ThenInclude(re => re.ExerciseTemplate)
                .Include(w => w.Exercises)
                .ToListAsync();

            // Get completed plan IDs
            var completedPlanIds = await _context.WorkoutProgress
                .Where(p => p.UserId == userId)
                .Select(p => p.WorkoutPlanId)
                .ToListAsync();

            var events = workoutPlans.Select(w => new
            {
                id = w.Id.ToString(),
                title = w.Title,
                start = w.Date.ToString("yyyy-MM-dd"),
                url = Url.Action("Details", new { id = w.Id }),
                workoutType = GetWorkoutType(w),
                completed = completedPlanIds.Contains(w.Id) // Pass completion status
            }).ToList();

            ViewBag.Events = JsonSerializer.Serialize(events);
            return View(workoutPlans);
        }

        private string GetWorkoutType(WorkoutPlan plan)
        {
            var regimenExercises = plan.Regimen?.Exercises.Select(re => re.ExerciseTemplate) ?? Enumerable.Empty<ExerciseTemplate>();
            var planExercises = plan.Exercises ?? Enumerable.Empty<Exercise>();

            var muscleGroups = regimenExercises.Select(e => e.MuscleGroup?.ToLower())
                .Concat(planExercises.Select(e => e.MuscleGroup?.ToLower()))
                .Where(g => !string.IsNullOrEmpty(g))
                .Distinct()
                .ToList();

            

            if (!muscleGroups.Any()) return "Cardio";

            bool hasUpper = muscleGroups.Any(g => g == "chest" || g == "back" || g == "shoulders" || g == "arms");
            bool hasLower = muscleGroups.Any(g => g == "quads" || g == "hamstrings" || g == "glutes" || g == "calves" || g == "legs");

            

            if (hasUpper && hasLower) return "FullBody";
            if (hasUpper) return "UpperBody";
            if (hasLower) return "LowerBody";
            return "Cardio";
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateDate([FromBody] UpdateDateModel payload)
        {
            
            var userId = _userManager.GetUserId(User);
            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(w => w.Id == payload.Id && w.UserId == userId);
            if (plan == null)
            {
                
                var allPlans = await _context.WorkoutPlans.ToListAsync();
                
                return NotFound();
            }
            
            plan.Date = payload.NewDate;
            await _context.SaveChangesAsync();
            
            return Ok();
        }



        // GET: WorkoutPlans/Create
        [Authorize]
        public IActionResult Create(string date = null)
        {
            var model = new WorkoutPlanCreateViewModel();
            if (!string.IsNullOrEmpty(date) && DateTime.TryParse(date, out var parsedDate))
            {
                model.Date = parsedDate;
            }
            ViewBag.Regimens = _context.Regimens.ToList();
            return View(model);
        }

        // POST: WorkoutPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(WorkoutPlanCreateViewModel model)
        {
            
            var userId = _userManager.GetUserId(User);
            
            

            if (ModelState.IsValid)
            {
                var workoutPlan = new WorkoutPlan
                {
                    Title = model.Title,
                    Date = model.Date,
                    UserId = userId,
                    RegimenId = model.RegimenId
                };
                _context.Add(workoutPlan);
                await _context.SaveChangesAsync();
                
                return RedirectToAction(nameof(Calendar));
            }

            
            ViewBag.Regimens = _context.Regimens.ToList();
            return View(model);
        }
        // GET: WorkoutPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan == null)
            {
                return NotFound();
            }
            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Date,UserId")] WorkoutPlan workoutPlan)
        {
            if (id != workoutPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workoutPlan);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkoutPlanExists(workoutPlan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(workoutPlan);
        }

        // GET: WorkoutPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workoutPlan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(m => m.Id == id);
            if (workoutPlan == null)
            {
                return NotFound();
            }

            return View(workoutPlan);
        }

        // POST: WorkoutPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workoutPlan = await _context.WorkoutPlans.FindAsync(id);
            if (workoutPlan != null)
            {
                _context.WorkoutPlans.Remove(workoutPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlan(int id)
        {
            var userId = _userManager.GetUserId(User);
            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
            if (plan == null) return NotFound();
            _context.WorkoutPlans.Remove(plan);
            await _context.SaveChangesAsync();
            
            return RedirectToAction(nameof(Calendar));
        }
        [Authorize]
        public IActionResult PlanRegimen()
        {
            ViewBag.Regimens = _context.Regimens.ToList();
            return View(new PlanRegimenViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> PlanRegimen(PlanRegimenViewModel model)
        {
            
            var userId = _userManager.GetUserId(User);
            

            if (ModelState.IsValid)
            {
                var regimen = await _context.Regimens.FindAsync(model.RegimenId);
                if (regimen == null)
                {
                    ModelState.AddModelError("", "Selected regimen not found.");
                    ViewBag.Regimens = _context.Regimens.ToList();
                    return View(model);
                }

                var plans = new List<WorkoutPlan>();
                var currentDate = model.StartDate;
                while (currentDate <= model.EndDate)
                {
                    if (model.SelectedDays.Contains(currentDate.DayOfWeek.ToString()))
                    {
                        plans.Add(new WorkoutPlan
                        {
                            Title = $"{regimen.Name} - {currentDate:MMM dd}",
                            Date = currentDate,
                            UserId = userId,
                            RegimenId = model.RegimenId
                        });
                    }
                    currentDate = currentDate.AddDays(1);
                }

                if (model.PreviewOnly)
                {
                    ViewBag.PreviewPlans = plans;
                }
                else
                {
                    _context.WorkoutPlans.AddRange(plans);
                    await _context.SaveChangesAsync();
                    
                    return RedirectToAction(nameof(Calendar));
                }
            }

            ViewBag.Regimens = _context.Regimens.ToList();
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> MarkComplete([FromBody] MarkCompleteRequest request)
        {
            int id = request?.Id ?? 0;
            
            var userId = _userManager.GetUserId(User);
            

            var plan = await _context.WorkoutPlans
                .FirstOrDefaultAsync(w => w.Id == id && w.UserId == userId);
            if (plan == null)
            {
                
                return NotFound();
            }

            // Check if already completed
            var existingProgress = await _context.WorkoutProgress
                .AnyAsync(p => p.WorkoutPlanId == id && p.UserId == userId);
            if (existingProgress)
            {
                
                return Ok(); // Or BadRequest("Already completed") 
            }

            var progress = new WorkoutProgress
            {
                UserId = userId,
                WorkoutPlanId = id,
                CompletedDate = DateTime.UtcNow
            };
            _context.WorkoutProgress.Add(progress);
            await _context.SaveChangesAsync();
            

            return Ok();
        }

        public class MarkCompleteRequest
        {
            public int Id { get; set; }
        }
        [Authorize]
        public async Task<IActionResult> Progress()
        {
            var userId = _userManager.GetUserId(User);
            var progress = await _context.WorkoutProgress
                .Include(p => p.WorkoutPlan)
                .Where(p => p.UserId == userId)
                .OrderByDescending(p => p.CompletedDate)
                .ToListAsync();
            return View(progress);
        }
        private bool WorkoutPlanExists(int id)
        {
            return _context.WorkoutPlans.Any(e => e.Id == id);
        }
    }
}
