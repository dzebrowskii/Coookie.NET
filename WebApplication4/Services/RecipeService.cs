using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication4.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO;

namespace WebApplication4.Services
{
    public class RecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Recipe>> RecipeSearcher(string ingredients)
        {
            var matchedRecipes = new List<Recipe>();
            var ingList = ingredients.ToLower().Split().ToList(); // List of ingredients from input, lowercase

            var recipes = await _context.Recipe
                .Include(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .ToListAsync();

            foreach (var recipe in recipes)
            {
                var recipeIngredients = recipe.RecipeIngredients.Select(ri => ri.Ingredient.Name.ToLower()).ToList();
                var commonIngredients = recipeIngredients.Intersect(ingList).ToList();

                if (commonIngredients.Count == recipeIngredients.Count)
                {
                    matchedRecipes.Add(recipe);
                }
            }

            return matchedRecipes;
        }
        
        public async Task SaveRecipeAsync(int userId, int recipeId)
        {
            var user = await _context.User.Include(u => u.FavoriteRecipes).FirstOrDefaultAsync(u => u.Id == userId);
            var recipe = await _context.Recipe.FindAsync(recipeId);

            if (user != null && recipe != null && !user.FavoriteRecipes.Contains(recipe))
            {
                recipe.DateSaved = DateTime.Now;
                recipe.Points += 1;
                user.FavoriteRecipes.Add(recipe);
                await _context.SaveChangesAsync();
            }
        }
        
        public async Task RemoveFavoriteRecipeAsync(int userId, int recipeId)
        {
            var user = await _context.User.Include(u => u.FavoriteRecipes).FirstOrDefaultAsync(u => u.Id == userId);
            var recipe = await _context.Recipe.FindAsync(recipeId);

            if (user != null && recipe != null && user.FavoriteRecipes.Contains(recipe))
            {
                user.FavoriteRecipes.Remove(recipe);
                await _context.SaveChangesAsync();
            }
        }

        
        
        

    }

}