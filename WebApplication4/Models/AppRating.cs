using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class AppRating
{
    [Key]
    public int? RatingId { get; set; }
    public int UserId { get; set; }  
    [Required]
    public virtual User User { get; set; } 

    [Required(ErrorMessage = "Rating value is required.")]
    [Range(1, 5, ErrorMessage = "Value must be between 1 and 5")]
    public int Value { get; set; }
    
    public DateTime? RatedOn { get; set; }
}
