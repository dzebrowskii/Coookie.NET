using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RecipeIngredientController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeIngredientController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipeIngredient
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecipeIngredient
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecipeIngredient/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredient
                .Include(r => r.Ingredient)
                .Include(r => r.Recipe)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            return View(recipeIngredient);
        }

        // GET: RecipeIngredient/Create
        public IActionResult Create()
        {
            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name");
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description");
            return View();
        }

        // POST: RecipeIngredient/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeIngredient recipeIngredient)
        {
            // Bez walidacji, po prostu dodaj do bazy danych
            _context.Add(recipeIngredient);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: RecipeIngredient/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeIngredient = await _context.RecipeIngredient.FindAsync(id);
            if (recipeIngredient == null)
            {
                return NotFound();
            }

            ViewData["IngredientId"] = new SelectList(_context.Ingredient, "Id", "Name", recipeIngredient.IngredientId);
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", recipeIngredient.RecipeId);
            return View(recipeIngredient);
        }

        // POST: RecipeIngredient/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeIngredient recipeIngredient)
        {
            if (id != recipeIngredient.RecipeId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(recipeIngredient);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeIngredientExists(recipeIngredient.RecipeId))
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
        
        public IActionResult Delete()
        {
            return View();
        }

        

        // POST: RecipeIngredient/Delete2/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int RecipeId, int IngredientId)
        {
            var recipeIngredient = await _context.RecipeIngredient.FindAsync(RecipeId,IngredientId);
            if (recipeIngredient != null)
            {
                _context.RecipeIngredient.Remove(recipeIngredient);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // Sprawdza, czy składnik receptury istnieje
        private bool RecipeIngredientExists(int id)
        {
            return _context.RecipeIngredient.Any(e => e.RecipeId == id);
        }
    }
}
