using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication4.Models;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Ingredient name is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 50 characters.")]
    public string Name { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}