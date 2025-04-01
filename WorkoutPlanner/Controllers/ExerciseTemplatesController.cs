using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutPlanner.Data;
using WorkoutPlanner.Models;
using Microsoft.EntityFrameworkCore;

namespace WorkoutPlanner.Controllers
{
    public class ExerciseTemplatesController : Controller
    {
        private readonly ApplicationDbContext _context;
        

        public ExerciseTemplatesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string searchString, string muscleGroup, string type)
        {
            var templates = _context.ExerciseTemplates.AsQueryable();
            if (!string.IsNullOrEmpty(searchString))
                templates = templates.Where(t => t.Name.Contains(searchString) || t.Description.Contains(searchString));
            if (!string.IsNullOrEmpty(muscleGroup))
                templates = templates.Where(t => t.MuscleGroup == muscleGroup);
            if (!string.IsNullOrEmpty(type))
                templates = templates.Where(t => t.Type == type);

            ViewBag.MuscleGroups = _context.ExerciseTemplates.Select(t => t.MuscleGroup).Distinct().ToList();
            ViewBag.Types = _context.ExerciseTemplates.Select(t => t.Type).Distinct().ToList();
            return View(templates.ToList());
        }

        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExerciseTemplate template)
        {
            if (ModelState.IsValid)
            {
                _context.ExerciseTemplates.Add(template);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Exercise template added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(template);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var template = await _context.ExerciseTemplates.FirstOrDefaultAsync(t => t.Id == id);
            if (template == null)
            {
                return NotFound();
            }

            return View(template);
        }
    }
}
