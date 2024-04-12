using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class AppRatingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppRatingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AppRating
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.AppRating.Include(a => a.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: AppRating/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRating = await _context.AppRating
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (appRating == null)
            {
                return NotFound();
            }

            return View(appRating);
        }

        // GET: AppRating/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: AppRating/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RatingId,UserId,Value")] AppRating appRating)
        {
            if (ModelState.IsValid)
            {
                _context.Add(appRating);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", appRating.UserId);
            return View(appRating);
        }

        // GET: AppRating/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRating = await _context.AppRating.FindAsync(id);
            if (appRating == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", appRating.UserId);
            return View(appRating);
        }

        // POST: AppRating/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RatingId,UserId,Value")] AppRating appRating)
        {
            if (id != appRating.RatingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appRating);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppRatingExists(appRating.RatingId))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", appRating.UserId);
            return View(appRating);
        }

        // GET: AppRating/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var appRating = await _context.AppRating
                .Include(a => a.User)
                .FirstOrDefaultAsync(m => m.RatingId == id);
            if (appRating == null)
            {
                return NotFound();
            }

            return View(appRating);
        }

        // POST: AppRating/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var appRating = await _context.AppRating.FindAsync(id);
            if (appRating != null)
            {
                _context.AppRating.Remove(appRating);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppRatingExists(int id)
        {
            return _context.AppRating.Any(e => e.RatingId == id);
        }
    }
}
