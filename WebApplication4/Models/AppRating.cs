using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models;
using System.ComponentModel.DataAnnotations;

public class AppRating
{
    [Key]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Ocena jest wymagana.")]
    [Range(1, 5, ErrorMessage = "Ocena musi być między 1 a 5.")]
    public int Value { get; set; }
    
    [Required]
    [ForeignKey("ApplicationUser")]
    public int UserId { get; set; }

    public virtual ApplicationUser User { get; set; }
}