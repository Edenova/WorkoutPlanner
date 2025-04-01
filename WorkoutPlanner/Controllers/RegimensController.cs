using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using WorkoutPlanner.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace WorkoutPlanner.Controllers
{
    public class RegimensController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        

        public RegimensController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var regimens = _context.Regimens
                .Include(r => r.Exercises)
                .ThenInclude(re => re.ExerciseTemplate)
                .ToList()
                .Select(r => new
                {
                    Regimen = r,
                    CanEdit = r.CreatedByUserId == userId
                }).ToList();

            ViewBag.CanEditDict = regimens.ToDictionary(r => r.Regimen.Id, r => r.CanEdit);
            return View(regimens.Select(r => r.Regimen).ToList());
        }
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.ExerciseTemplates = _context.ExerciseTemplates.ToList();
            return View(new RegimenCreateViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegimenCreateViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                var regimen = new Regimen
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Exercises = model.Exercises
                        .Where(e => e.ExerciseTemplateId > 0)
                        .Select(e => new RegimenExercise
                        {
                            ExerciseTemplateId = e.ExerciseTemplateId,
                            Sets = e.Sets,
                            Reps = e.Reps
                        }).ToList()
                };

                _context.Regimens.Add(regimen);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Regimen created successfully!";
                return RedirectToAction(nameof(Index));
            }

            
            ViewBag.ExerciseTemplates = _context.ExerciseTemplates.ToList();
            return View(model);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regimen = await _context.Regimens
                .Include(r => r.Exercises)
                .ThenInclude(re => re.ExerciseTemplate)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (regimen == null)
            {
                return NotFound();
            }

            var creator = await _userManager.FindByIdAsync(regimen.CreatedByUserId);
            ViewBag.CreatorName = creator?.UserName ?? "Unknown";
            return View(regimen);
        }
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regimen = await _context.Regimens
                .Include(r => r.Exercises)
                .ThenInclude(re => re.ExerciseTemplate)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (regimen == null)
            {
                return NotFound();
            }

            if (regimen.CreatedByUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid(); // Only creator can edit
            }

            ViewBag.ExerciseTemplates = _context.ExerciseTemplates.ToList();
            return View(new RegimenCreateViewModel
            {
                Name = regimen.Name,
                Description = regimen.Description,
                Exercises = regimen.Exercises.Select(re => new RegimenCreateViewModel.RegimenExerciseInput
                {
                    ExerciseTemplateId = re.ExerciseTemplateId,
                    Sets = re.Sets,
                    Reps = re.Reps
                }).ToList()
            });
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RegimenCreateViewModel model)
        {
            var regimen = await _context.Regimens
                .Include(r => r.Exercises)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (regimen == null)
            {
                return NotFound();
            }

            if (regimen.CreatedByUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                regimen.Name = model.Name;
                regimen.Description = model.Description;

                // Update exercises
                _context.RegimenExercises.RemoveRange(regimen.Exercises);
                regimen.Exercises = model.Exercises
                    .Where(e => e.ExerciseTemplateId > 0)
                    .Select(e => new RegimenExercise
                    {
                        ExerciseTemplateId = e.ExerciseTemplateId,
                        Sets = e.Sets,
                        Reps = e.Reps
                    }).ToList();

                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Regimen updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ExerciseTemplates = _context.ExerciseTemplates.ToList();
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regimen = await _context.Regimens
                .Include(r => r.Exercises)
                .ThenInclude(re => re.ExerciseTemplate) 
                .FirstOrDefaultAsync(r => r.Id == id);

            if (regimen == null)
            {
                return NotFound();
            }

            if (regimen.CreatedByUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return Forbid();
            }

            return View(regimen);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var regimen = await _context.Regimens.FindAsync(id);
            if (regimen == null || regimen.CreatedByUserId != User.FindFirstValue(ClaimTypes.NameIdentifier))
            {
                return NotFound();
            }

            _context.Regimens.Remove(regimen);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Regimen deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
