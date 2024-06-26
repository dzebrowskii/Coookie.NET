using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.Services;
using System.Threading.Tasks;

namespace WebApplication4.Controllers
{
    public class AppRatingController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;

        public AppRatingController(ApplicationDbContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        // GET: AppRating/Rate
        [HttpGet]
        public async Task<IActionResult> Rate()
        {
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);

            if (await _userService.HasUserRatedAppAsync(user.Id))
            {
                ViewBag.Message = "You have already rated the app.";
                return View("AlreadyRated");
            }

            return View();
        }

        // POST: AppRating/Rate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Rate(int value)
        {
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (await _userService.HasUserRatedAppAsync(user.Id))
            {
                ViewBag.Message = "You have already rated the app.";
                return View("AlreadyRated");
            }

            await _userService.AddUserRatingAsync(user.Id, value);

            TempData["SuccessMessage"] = "Thank you for rating the app!";
            return RedirectToAction("Menu", "User");
        }

        // GET: AppRating
        public async Task<IActionResult> Index()
        {
            return View(await _context.AppRating.Include(ar => ar.User).ToListAsync());
        }

        public IActionResult Edit()
        {
            return View();
        }

        public IActionResult Delete()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
