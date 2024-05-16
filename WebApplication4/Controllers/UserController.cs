using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;

        public UserController(ApplicationDbContext context, EmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: User
        public async Task<IActionResult> Index()
        {
            return View(await _context.User.ToListAsync());
        }

        // GET: User/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Username,UserSurname,Password")] User user)
        {
            user.ActivationToken = Guid.NewGuid().ToString();
            user.IsActive = false;

            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
            }

            if (!ModelState.IsValid)
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        Console.WriteLine($"Error in {modelStateKey}: {error.ErrorMessage}");
                    }
                }

                return View(user);
            }

            _context.Add(user);
            await _context.SaveChangesAsync();

            await SendActivationEmail(user);

            TempData["SuccessMessage"] = $"Account was created for {user.Email}. Please check your email to activate your account.";
            return RedirectToAction("Login");
        }

        private async Task SendActivationEmail(User user)
        {
            var activationLink = Url.Action("ActivateAccount", "User", new { token = user.ActivationToken }, Request.Scheme);
            string subject = "Account Activation";
            string message = $"Please activate your account by clicking the following link: {activationLink}";
            await _emailService.SendEmailAsync(user.Email, subject, message);
        }

        public async Task<IActionResult> ActivateAccount(string token)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.ActivationToken == token);
            if (user == null)
            {
                return BadRequest("Invalid activation token.");
            }

            user.IsActive = true;
            user.ActivationToken = null;

            await _context.SaveChangesAsync();

            return Ok("Account activated successfully.");
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult GuestApp()
        {
            return View();
        }

        public IActionResult LoggedApp()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string email, string password)
        {
            var user = _context.User.FirstOrDefault(u => u.Email == email);
            if (user != null && user.Password == password)
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty, "Your account is not activated. Please check your email to activate your account.");
                    return View();
                }
                return RedirectToAction("LoggedApp");
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View();
        }

        // GET: User/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Username,UserSurname,Password")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: User/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user != null)
            {
                _context.User.Remove(user);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
