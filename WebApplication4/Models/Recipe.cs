using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
namespace WebApplication4.Models;



public class Recipe
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Recipe name is required.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(1000, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 1000 characters.")]
    public string Description { get; set; }

    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public virtual ICollection<RecipeRanking> RecipeRatings { get; set; } = new List<RecipeRanking>();
    
    // Właściwość dla relacji wiele-do-wielu z User
        public virtual ICollection<User> Users { get; set; } = new List<User>();
}
