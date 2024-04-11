using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class RecipeIngredient
{
    [Key]
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    [Key]
    public Guid IngredientId { get; set; }
    public virtual Ingredient Ingredient { get; set; }
}