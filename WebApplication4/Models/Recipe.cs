using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class Recipe
{
    [Key]
    private int RecipeId { get; set; }
    
}