using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models;

public class RecipeRanking
{
    
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; }

    [Required(ErrorMessage = "Rating value is required.")]
    [Range(1, 5, ErrorMessage = "Value must be between 1 and 5")]
    public int Value { get; set; }
}
