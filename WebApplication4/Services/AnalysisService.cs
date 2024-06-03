using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApplication4.Services
{
    public class AnalysisService
    {
        private readonly ApplicationDbContext _context;

        public AnalysisService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, int>> GetIngredientUsageAsync(int userId)
        {
            var user = await _context.User
                .Include(u => u.FavoriteRecipes)
                .ThenInclude(r => r.RecipeIngredients)
                .ThenInclude(ri => ri.Ingredient)
                .FirstOrDefaultAsync(u => u.Id == userId);

            var ingredientCounts = new Dictionary<string, int>();

            foreach (var recipe in user.FavoriteRecipes)
            {
                foreach (var recipeIngredient in recipe.RecipeIngredients)
                {
                    var ingredientName = recipeIngredient.Ingredient.Name;
                    if (ingredientCounts.ContainsKey(ingredientName))
                    {
                        ingredientCounts[ingredientName]++;
                    }
                    else
                    {
                        ingredientCounts[ingredientName] = 1;
                    }
                }
            }

            return ingredientCounts;
        }
    }
}
