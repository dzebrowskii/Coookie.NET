using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApplication4.Data;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class RecipeScraper
    {
        private static readonly HttpClient httpClient = new HttpClient();
        private readonly ApplicationDbContext _context;

        public RecipeScraper(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ScrapeAsync()
        {
            string url = "https://www.smaczny.pl/przepisy,codzienne_gotowanie";
            string html = await httpClient.GetStringAsync(url);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);
            var whole_recipe_links = htmlDocument.DocumentNode.SelectNodes("//a[contains(@class, 'p_lista_a')]");
            List<string> links = new List<string>();

            if (whole_recipe_links != null)
            {
                foreach (var node in whole_recipe_links)
                {
                    string href = node.GetAttributeValue("href", string.Empty);
                    if (!string.IsNullOrEmpty(href))
                    {
                        // Dodaj pełny adres URL
                        links.Add(new Uri(new Uri(url), href).ToString());
                    }
                }
            }

            foreach (string item in links)
            {
                string html_recipe = await httpClient.GetStringAsync(item);
                HtmlDocument html_recipe_Document = new HtmlDocument();
                html_recipe_Document.LoadHtml(html_recipe);

                // Recipe name
                var recipeNameNode = html_recipe_Document.DocumentNode.SelectSingleNode("//h1");
                string recipeName = recipeNameNode?.InnerText.Trim() ?? "Untitled Recipe";
                Console.WriteLine(recipeName);

                // Sprawdź, czy przepis już istnieje w bazie danych
                bool recipeExists = await _context.Recipe.AnyAsync(r => r.Name == recipeName);
                if (recipeExists)
                {
                    continue; // Przejdź do następnego przepisu, jeśli już istnieje
                }

                var ingredientsNodes = html_recipe_Document.DocumentNode.SelectNodes("//div[@itemprop='recipeIngredient']");
                List<string> recipeIngredients = new List<string>();

                foreach (var x in ingredientsNodes)
                {
                    string ingredientText = x.InnerText.Trim();
                    ingredientText = System.Text.RegularExpressions.Regex.Replace(ingredientText, @"\d", string.Empty);
                    recipeIngredients.Add(ingredientText);
                    Console.WriteLine(ingredientText);
                }

                Console.WriteLine(recipeIngredients.Count);

                var recipeInstructionsNodes = html_recipe_Document.DocumentNode.SelectNodes("//div[contains(@class, 'wykonanie instructions')]/p");
                List<string> recipeInstructions = new List<string>();

                foreach (var y in recipeInstructionsNodes)
                {
                    string recipeText = y.InnerText.Trim();
                    recipeInstructions.Add(recipeText);
                    Console.WriteLine(recipeText);
                }

                Console.WriteLine(recipeInstructions.Count);
                Console.WriteLine("--------------------");

                // Połącz instrukcje w jeden ciąg tekstowy
                string combinedInstructions = string.Join("\n", recipeInstructions);

                // Przekształć listę składników na ICollection<RecipeIngredient>
                ICollection<RecipeIngredient> recipeIngredientEntities = new List<RecipeIngredient>();

                foreach (var ingredientName in recipeIngredients)
                {
                    // Znajdź lub utwórz składnik
                    var ingredient = await _context.Ingredient
                        .FirstOrDefaultAsync(i => i.Name == ingredientName);
                    if (ingredient == null)
                    {
                        ingredient = new Ingredient { Name = ingredientName };
                        _context.Ingredient.Add(ingredient);
                        await _context.SaveChangesAsync();
                    }

                    // Utwórz RecipeIngredient
                    recipeIngredientEntities.Add(new RecipeIngredient { Ingredient = ingredient });
                }

                // Utwórz nowy obiekt Recipe i dodaj go do kontekstu
                Recipe recipe = new Recipe
                {
                    Name = recipeName,
                    Description = combinedInstructions,
                    RecipeIngredients = recipeIngredientEntities
                };

                _context.Recipe.Add(recipe);
                await _context.SaveChangesAsync();

                // Zakończ działanie scrappera po zapisaniu pierwszego przepisu
                Console.WriteLine("Zapisano przepis i zakończono działanie scrappera.");
                return;
            }

            Console.WriteLine("Nie znaleziono nowych przepisów do zapisania.");
        }
    }
}
