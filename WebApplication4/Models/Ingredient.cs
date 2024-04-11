using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class Ingredient
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Name cannot be longer than 50 characters.")]
    public string Name { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
}