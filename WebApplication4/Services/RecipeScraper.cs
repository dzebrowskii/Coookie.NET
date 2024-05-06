using HtmlAgilityPack;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApplication4.Services
{
    public class RecipeScraper
    {
        private static readonly HttpClient httpClient = new HttpClient();

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
                var ingredients = html_recipe_Document.DocumentNode.SelectNodes("//div[@itemprop='recipeIngredient']");
                List<string> recipe_ingredients = new List<string>();
                
                foreach (var x in ingredients)
                {
                    string ingredientText = x.InnerText.Trim();
                    recipe_ingredients.Add(ingredientText); 
                    Console.WriteLine(ingredientText); 
                }//Tu juz jest lista ze wszystkimi skladnikami (recipe_ingeredients)
                
                
                var recipeInstructions = html_recipe_Document.DocumentNode.SelectNodes("//div[contains(@class, 'wykonanie instructions')]/p");
                List<string> recipe_instructions = new List<string>();
                foreach (var y in recipeInstructions)
                {
                    string recipeText = y.InnerText.Trim();
                    recipe_instructions.Add(recipeText); 
                    Console.WriteLine(recipeText); // tu do listy dodaja sie wszystkie instrukcje przygotowania
                }
                Console.WriteLine("--------------------"); 
            
            }
            
        }
    }
}