using Microsoft.AspNetCore.Mvc;
using WebApplication4.Services;

namespace WebApplication4.Controllers;

public class GuestAppController : Controller
{
    
    private readonly RecipeService _recipeService;
    
    public GuestAppController(RecipeService recipeService
        )
    {
        
        _recipeService = recipeService;
        
            
    }
    
    public IActionResult GuestApp()
    {
        return View();
    }
    public async Task<IActionResult> FindRecipes(string ingredients)
    {
        var matchedRecipes = await _recipeService.RecipeSearcher(ingredients);
            
        if (matchedRecipes == null || !matchedRecipes.Any())
        {
            TempData["NoResultsMessage"] = "Unfortunately, we have not matched any of the recipes to the given ingredients.";
        }

        return View("GuestApp", matchedRecipes);
    }
}