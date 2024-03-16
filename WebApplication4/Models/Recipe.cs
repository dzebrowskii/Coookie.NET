using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class Recipe
{
    [Key]
    public int RecipeId { get; set; }
    
    [Required]
    public string Name { get; set; }
    
    [StringLength(1000)]
    private string Description { get; set; }
    
    // Relacja wiele-do-wielu z Ingredient
    public virtual ICollection<Ingredient> Ingredients { get; set; }
    
    public Recipe()
    {
        Ingredients = new HashSet<Ingredient>();
    }
    

}