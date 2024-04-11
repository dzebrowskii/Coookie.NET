using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters.")]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }
    
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; }
    public virtual ICollection<RecipeRanking> RecipeRatings { get; set; }
}