using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class AppRating
{
    [Key]
    public int RatingId { get; set; }

    public int UserId { get; set; }  // Usunięcie [Required] z typu wartościowego
    [Required]
    public virtual User User { get; set; }  // Zapewnienie obecności obiektu User

    [Required(ErrorMessage = "Rating value is required.")]
    [Range(1, 5, ErrorMessage = "Value must be between 1 and 5")]
    public int Value { get; set; }
}
