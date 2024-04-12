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
    public class UserRankingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserRankingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserRanking
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserRanking.Include(u => u.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserRanking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRanking = await _context.UserRanking
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRanking == null)
            {
                return NotFound();
            }

            return View(userRanking);
        }

        // GET: UserRanking/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email");
            return View();
        }

        // POST: UserRanking/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,RankPoints,Badges")] UserRanking userRanking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userRanking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", userRanking.UserId);
            return View(userRanking);
        }

        // GET: UserRanking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRanking = await _context.UserRanking.FindAsync(id);
            if (userRanking == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", userRanking.UserId);
            return View(userRanking);
        }

        // POST: UserRanking/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,RankPoints,Badges")] UserRanking userRanking)
        {
            if (id != userRanking.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userRanking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserRankingExists(userRanking.UserId))
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
            ViewData["UserId"] = new SelectList(_context.User, "Id", "Email", userRanking.UserId);
            return View(userRanking);
        }

        // GET: UserRanking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userRanking = await _context.UserRanking
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (userRanking == null)
            {
                return NotFound();
            }

            return View(userRanking);
        }

        // POST: UserRanking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userRanking = await _context.UserRanking.FindAsync(id);
            if (userRanking != null)
            {
                _context.UserRanking.Remove(userRanking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserRankingExists(int id)
        {
            return _context.UserRanking.Any(e => e.UserId == id);
        }
    }
}
