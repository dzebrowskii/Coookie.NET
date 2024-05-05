using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RecipeRankingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeRankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: RecipeRanking
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.RecipeRanking
                .Include(r => r.Recipe)
                .Include(r => r.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: RecipeRanking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeRanking = await _context.RecipeRanking
                .Include(r => r.Recipe)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeRanking == null)
            {
                return NotFound();
            }

            return View(recipeRanking);
        }

        // GET: RecipeRanking/Create
        public IActionResult Create()
        {
            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description");
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: RecipeRanking/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecipeRanking recipeRanking)
        {
            // Bez sprawdzania ModelState, dodaj obiekt bezpośrednio
            _context.Add(recipeRanking);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: RecipeRanking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeRanking = await _context.RecipeRanking.FindAsync(id);
            if (recipeRanking == null)
            {
                return NotFound();
            }

            ViewData["RecipeId"] = new SelectList(_context.Recipe, "Id", "Description", recipeRanking.RecipeId);
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", recipeRanking.UserId);
            return View(recipeRanking);
        }

        // POST: RecipeRanking/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RecipeRanking recipeRanking)
        {
            if (id != recipeRanking.RecipeId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(recipeRanking);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RecipeRankingExists(recipeRanking.RecipeId))
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

        // GET: RecipeRanking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipeRanking = await _context.RecipeRanking
                .Include(r => r.Recipe)
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.RecipeId == id);
            if (recipeRanking == null)
            {
                return NotFound();
            }

            return View(recipeRanking);
        }

        // POST: RecipeRanking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipeRanking = await _context.RecipeRanking.FindAsync(id);
            if (recipeRanking != null)
            {
                _context.RecipeRanking.Remove(recipeRanking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeRankingExists(int id)
        {
            return _context.RecipeRanking.Any(e => e.RecipeId == id);
        }
    }
}
