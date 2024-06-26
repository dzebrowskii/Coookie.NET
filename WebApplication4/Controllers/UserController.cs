using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using WebApplication4.Services;
using WebApplication4.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;

namespace WebApplication4.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly EmailService _emailService;
        private readonly RecipeService _recipeService;
        private readonly UserService _userService;
        

        public UserController(ApplicationDbContext context, EmailService emailService, RecipeService recipeService,
            UserService userService)
        {
            _context = context;
            _emailService = emailService;
            _recipeService = recipeService;
            _userService = userService;
            
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

        // GET: User/Registration
        public IActionResult Registration()
        {
            return View();
        }

        // POST: User/Registration
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration([Bind("Id,Email,Username,UserSurname,Password,ReferredBy")] User user)
        {
            user.ActivationToken = Guid.NewGuid().ToString();
            user.IsActive = false;
            user.Points = 0;
            
            // Generowanie unikalnego kodu polecającego
            user.ReferralCode = GenerateReferralCode(user.Email);
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
            }

            if (!ModelState.IsValid)
            {
                
                return View(user);
            }
            // Sprawdzanie kodu polecającego i przyznawanie punktów
            if (!string.IsNullOrEmpty(user.ReferredBy))
            {
                var referringUser = await _context.User.FirstOrDefaultAsync(u => u.ReferralCode == user.ReferredBy);
                if (referringUser != null)
                {
                    referringUser.Points += 10; 
                    user.Points += 5; 
                    _context.User.Update(referringUser);
                }
            }

            _context.Add(user);
            await _context.SaveChangesAsync();

            await SendActivationEmail(user);

            TempData["SuccessMessage"] =
                $"Account was created for {user.Email}. Please check your email to activate your account.";
            return RedirectToAction("Login");
        }
        
        
        
        private string GenerateReferralCode(string email)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hash = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(email));
                return BitConverter.ToString(hash).Replace("-", "").Substring(0, 10);
            }
        }
        
        

        private async Task SendActivationEmail(User user)
        {
            var activationLink = Url.Action("ActivateAccount", "User", new { token = user.ActivationToken },
                Request.Scheme);
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
        
       
        [HttpPost]
        public async Task<IActionResult> SaveRecipe(int recipeId)
        {
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);

            if (user != null)
            {
                await _recipeService.SaveRecipeAsync(user.Id, recipeId);
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        
        

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                if (!user.IsActive)
                {
                    ModelState.AddModelError(string.Empty,
                        "Your account is not activated. Please check your email to activate your account.");
                    return View();
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email)
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    // AllowRefresh = true,
                    // Refreshing the authentication session should be allowed.

                    // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    // IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    // IssuedUtc = <DateTimeOffset.UtcNow>,
                    // The time at which the authentication ticket was issued.

                    // RedirectUri = <String>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                return RedirectToAction("LoggedApp", "LoggedApp");
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

        // GET: User/ResetPassword
        public IActionResult ResetPassword()
        {
            return View();
        }

        // POST: User/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "There is no account associated with this email.");
                    return View(model);
                }

                var token = Guid.NewGuid().ToString();
                user.ActivationToken = token; 
                await _context.SaveChangesAsync();

                var resetLink = Url.Action("ChangePassword", "User", new { token = token }, Request.Scheme);
                string subject = "Password Reset";
                string message = $"Please reset your password by clicking the following link: {resetLink}";
                await _emailService.SendEmailAsync(user.Email, subject, message);

                TempData["SuccessMessage"] =
                    "An email with a password reset link has been sent. Please check your inbox to reset your password.";
                return RedirectToAction("ResetPasswordConfirmation");
            }

            return View(model);
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        // GET: User/ChangePassword
        public IActionResult ChangePassword(string token)
        {
            var model = new ChangePasswordViewModel { Token = token };
            return View();
        }

        // POST: User/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.ActivationToken == model.Token);
                if (user == null)
                {
                    ModelState.AddModelError(string.Empty, "Invalid token.");
                    return View();
                }

                user.Password = model.Password;
                user.ActivationToken = null; // Clear the token
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Your password has been reset. Please log in with your new password.";
                return RedirectToAction("Login");
            }

            return View();
        }

        public IActionResult Menu()
        {
            return View();
        }

        
        
        



    }
    
    
    
    
}
