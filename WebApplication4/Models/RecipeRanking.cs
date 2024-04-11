using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class RecipeRanking
{
    [Key]
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    [Required]
    public int Points { get; set; }
}