using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class RecipeService
    {
        private readonly ApplicationDbContext _context;

        public RecipeService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<string>> RecipeSearcher(string ingredients)
        {
            var matchedRecipes = new List<string>();
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
                    matchedRecipes.Add(recipe.Description);
                }
            }

            if (matchedRecipes.Count == 0)
            {
                matchedRecipes.Add("Unfortunately, we have not matched any of the recipes to the given ingredients");
            }

            return matchedRecipes;
        }
    }
}