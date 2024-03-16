using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class RecipeRanking
{
    [Key]
    public int RecipeID { get; set; } // Używane jako klucz główny
    public int RecipePlace { get; set; }

    // Nawigacja do Recipe
    public virtual Recipe Recipe { get; set; }
}