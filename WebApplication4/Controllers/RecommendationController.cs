using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using WebApplication4.Models;
using WebApplication4.Services; 

namespace WebApplication4.Controllers
{
    [Authorize]
    public class RecommendationController : Controller
    {
        private readonly UserService _userService;

        public RecommendationController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> AppRecommendation()
        {
            
            var email = User.Identity.Name;
            var user = await _userService.GetUserByEmailAsync(email);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            ViewData["ReferralCode"] = user.ReferralCode;
            return View();
        }
    }
}