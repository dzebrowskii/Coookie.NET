using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class LoggedAppController : Controller
    {
        private readonly RecipeService _recipeService;

        public LoggedAppController(RecipeService recipeService)
        {
            _recipeService = recipeService;
        }
        
        public IActionResult LoggedApp()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FindRecipes(string ingredients, string returnView)
        {
            var matchedRecipes = await _recipeService.RecipeSearcher(ingredients);

            if (!matchedRecipes.Any())
            {
                TempData["NoResultsMessage"] = "Unfortunately, we have not matched any of the recipes to the given ingredients.";
            }

            return View("LoggedApp", matchedRecipes);
        }
    }
}