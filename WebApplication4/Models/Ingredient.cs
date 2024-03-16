namespace WebApplication4.Models;
using System.ComponentModel.DataAnnotations;
public class Ingredient
{
    [Key]
    public int IngredientId { get; set; }

    [StringLength(255)]
    public string Name { get; set; }
    
    // Relacja wiele-do-wielu z Recipe
    public virtual ICollection<Recipe> Recipes { get; set; }
    
    public Ingredient()
    {
        Recipes = new HashSet<Recipe>();
    }
    
}