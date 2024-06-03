using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class AnalysisController : Controller
    {
        private readonly AnalysisService _analysisService;
        private readonly UserService _userService;

        public AnalysisController(AnalysisService analysisService, UserService userService)
        {
            _analysisService = analysisService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult FoodAnalysis()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FoodAnalysis2()
        {
            var email = User.Identity.Name;
            var userId = await _userService.GetUserByEmailAsync(email);

            if (userId == null)
            {
                return BadRequest("User not found.");
            }

            var ingredientUsage = await _analysisService.GetIngredientUsageAsync(userId.Id);

            return Json(ingredientUsage);
        }
    }
}