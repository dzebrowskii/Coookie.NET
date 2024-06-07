using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Services;
using WebApplication4.Models;
using WebApplication4.ViewModels;

namespace WebApplication4.Controllers
{
    public class MyAccountController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        private readonly EmailService _emailService;

        public MyAccountController(ApplicationDbContext context, UserService userService, EmailService emailService)
        {
            _context = context;
            _userService = userService;
            _emailService = emailService;
        }

        // GET: MyAccount/ChangePasswordLoggedUser
        [HttpGet]
        public IActionResult ChangePasswordLoggedUser()
        {
            return View();
        }

        // POST: MyAccount/ChangePasswordLoggedUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordLoggedUser(ChangePasswordForLoggedInUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            if (user.Password != model.CurrentPassword)
            {
                ModelState.AddModelError(string.Empty, "Current password is incorrect.");
                return View(model);
            }

            user.Password = model.NewPassword;
            _context.Update(user);
            await _context.SaveChangesAsync();

            TempData["PasswordChange"] = "Password changed successfully.";
            return RedirectToAction("MyAccount", "LoggedUserOptions");
        }

        // GET: MyAccount/ChangeEmail
        public IActionResult ChangeEmail()
        {
            return View();
        }

        // POST: MyAccount/ChangeEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeEmail(string newEmail)
        {
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound("User not found.");
            }

            var existingUser = await _userService.GetUserByEmailAsync(newEmail);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View();
            }

            user.ActivationToken = Guid.NewGuid().ToString();
            user.Email = newEmail;
            user.IsActive = false;

            await _context.SaveChangesAsync();
            await SendEmailChangeActivationEmail(user);

            TempData["SuccessMessage"] = $"An email change confirmation link has been sent to {newEmail}. Please check your email to confirm the change.";
            return RedirectToAction("Login", "User");
        }

        private async Task SendEmailChangeActivationEmail(User user)
        {
            var activationLink = Url.Action("ActivateAccount", "User", new { token = user.ActivationToken }, Request.Scheme);
            string subject = "Email Change Activation";
            string message = $"Please confirm your email change by clicking the following link: {activationLink}";
            await _emailService.SendEmailAsync(user.Email, subject, message);
        }

        // GET: MyAccount/DeleteAccount
        public IActionResult DeleteAccount()
        {
            return View();
        }

        // POST: MyAccount/DeleteAccountConfirmed
        [HttpPost, ActionName("DeleteAccount")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccountConfirmed()
        {
            var email = User.Identity.Name;
            await _userService.DeleteAccountAsync(email);

            return RedirectToAction("Index", "Home");
        }
    }
}
