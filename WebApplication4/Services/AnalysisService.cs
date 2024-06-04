using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication4.Models;
using WebApplication4.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;


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
        
        public async Task<Dictionary<string, decimal>> GenerateAnalysisDataAsync(int userId, Func<Recipe, decimal?> selector)
        {
            var user = await _context.User
                .Include(u => u.FavoriteRecipes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                throw new Exception("User not found");
            }

            Console.WriteLine("Favorite Recipes Count: " + user.FavoriteRecipes.Count);

            foreach (var recipe in user.FavoriteRecipes)
            {
                Console.WriteLine($"Recipe: {recipe.Name}, Value: {selector(recipe)}, DateSaved: {recipe.DateSaved}");
            }

            var analysisData = user.FavoriteRecipes
                .Where(r => r.DateSaved.HasValue && selector(r).HasValue)
                .GroupBy(r => r.DateSaved.Value.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    TotalValue = g.Sum(r => selector(r).Value)
                })
                .ToDictionary(x => x.Date.ToString("yyyy-MM-dd"), x => x.TotalValue);

            // Logowanie danych
            Console.WriteLine("Analysis Data:");
            foreach (var item in analysisData)
            {
                Console.WriteLine($"{item.Key}: {item.Value}");
            }

            return analysisData;
        }

        public async Task<Dictionary<string, decimal>> GenerateFinancialAnalysisDataAsync(int userId)
        {
            return await GenerateAnalysisDataAsync(userId, r => r.Price);
        }

        public async Task<Dictionary<string, decimal>> GenerateCaloricAnalysisDataAsync(int userId)
        {
            return await GenerateAnalysisDataAsync(userId, r => r.Calories);
        }

    }
}
