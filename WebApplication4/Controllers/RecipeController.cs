using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly RecipeScraper _scraper;
        private readonly UserService _userService;

        
        public RecipeController(ApplicationDbContext context, RecipeScraper scraper, UserService userService)
        {
            _context = context;
            _scraper = scraper;
            _userService = userService;
        }

        // GET: Recipe
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipe.ToListAsync());
        }

        // GET: Recipe/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // GET: Recipe/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipe/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Calories, Price,Points")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();

               
                var email = User.Identity.Name;
                var user = await _userService.GetUserByEmailAsync(email);
                if (user != null)
                {
                    
                    user.Points += 20; 
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }
            return View(recipe);

        }

        // GET: Recipe/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe == null)
            {
                return NotFound();
            }
            return View(recipe);
        }

        // POST: Recipe/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            return View(recipe);
        }

        // GET: Recipe/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipe
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipe/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recipe = await _context.Recipe.FindAsync(id);
            if (recipe != null)
            {
                _context.Recipe.Remove(recipe);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        // GET: Recipe/AddRecipe
        [HttpGet]
        public IActionResult AddRecipe()
        {
            return View();
        }

        // POST: Recipe/AddRecipe
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecipe([Bind("Name,Description,Ingredients,Calories,Price")] Recipe recipe, string Ingredients)
        {
            if (ModelState.IsValid)
            {
                // Przetwarzanie składników
                var ingredientNames = Ingredients.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var ingredientName in ingredientNames)
                {
                    var ingredient = new Ingredient { Name = ingredientName.Trim() };
                    var recipeIngredient = new RecipeIngredient { Ingredient = ingredient, Recipe = recipe };
                    recipe.RecipeIngredients.Add(recipeIngredient);
                    recipe.Points = 0;
                }

                _context.Add(recipe);
                await _context.SaveChangesAsync();

                // Dodawanie punktów użytkownikowi za dodanie przepisu
                var email = User.Identity.Name;
                var user = await _userService.GetUserByEmailAsync(email);
                if (user != null)
                {
                    user.Points += 20; 
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }

                TempData["newRecipe"] = "Recipe successfully added.";
                return RedirectToAction("Menu","Recipe");
            }
            return View(recipe);
        }

        private bool RecipeExists(int id)
        {
            return _context.Recipe.Any(e => e.Id == id);
        }

        // Metoda do uruchomienia scraper
        public async Task<IActionResult> Scrape()
        {
            await _scraper.ScrapeAsync();
            return Content("Scraping wykonany.");
        }
        
        public IActionResult Menu()
        {
            return View();
        }
        public async Task<IActionResult> AddBasedOnExisting()
        {
            var recipes = await _context.Recipe.ToListAsync();
            ViewBag.Recipes = recipes;
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> AddBasedOnExistingStep2(int existingRecipeId)
        {
            var existingRecipe = await _context.Recipe
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(r => r.Id == existingRecipeId);

            if (existingRecipe == null)
            {
                return NotFound("Recipe not found.");
            }

            ViewBag.ExistingRecipe = existingRecipe;
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        [HttpPost]
        public async Task<IActionResult> SaveBasedOnExisting(int existingRecipeId, string name, string newIngredients, string description, int calories, decimal price, List<string> existingIngredients)
{
    var existingRecipe = await _context.Recipe
        .Include(r => r.RecipeIngredients)
        .ThenInclude(ri => ri.Ingredient)
        .FirstOrDefaultAsync(r => r.Id == existingRecipeId);

    if (existingRecipe == null)
    {
        return NotFound("Recipe not found.");
    }

    var newRecipe = new Recipe
    {
        Name = name,
        Description = description,
        Calories = calories,
        Price = price,
        RecipeIngredients = new List<RecipeIngredient>()
    };

    // Kopiowanie istniejących składników do nowego przepisu, jeśli są w liście existingIngredients
    foreach (var existingRecipeIngredient in existingRecipe.RecipeIngredients)
    {
        if (existingIngredients.Contains(existingRecipeIngredient.Ingredient.Name))
        {
            var newRecipeIngredient = new RecipeIngredient
            {
                Recipe = newRecipe,
                Ingredient = existingRecipeIngredient.Ingredient
            };
            newRecipe.RecipeIngredients.Add(newRecipeIngredient);
        }
    }

    // Dodawanie nowych składników
    if (!string.IsNullOrEmpty(newIngredients))
    {
        var ingredientsArray = newIngredients.Split(',');
        foreach (var ingredientName in ingredientsArray)
        {
            var ingredient = await _context.Ingredient.FirstOrDefaultAsync(i => i.Name == ingredientName.Trim());
            if (ingredient == null)
            {
                ingredient = new Ingredient { Name = ingredientName.Trim() };
                _context.Ingredient.Add(ingredient);
            }
            newRecipe.RecipeIngredients.Add(new RecipeIngredient { Recipe = newRecipe, Ingredient = ingredient });
        }
    }

    _context.Recipe.Add(newRecipe);
    await _context.SaveChangesAsync();
    TempData["newRecipe"] = "Recipe successfully added.";

    return RedirectToAction("Menu", "Recipe");
}


        
        

    }
}
