namespace WebApplication4.Models;
using System.ComponentModel.DataAnnotations;
public class Ingredient
{
    [Key]
    public int IngredientId { get; set; }

    [StringLength(255)]
    public string Name { get; set; }
    
    public virtual ICollection<Recipe> Recipes { get; set; }
    
}