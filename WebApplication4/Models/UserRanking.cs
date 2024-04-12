using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication4.Models;

public class UserRanking
{
    [Key]
    [ForeignKey("User")]
    public int UserId { get; set; }
    public virtual User User { get; set; }

    [Required(ErrorMessage = "Rank points are required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Rank points must be a non-negative number.")]
    public int RankPoints { get; set; }

    [StringLength(255, ErrorMessage = "Badges string cannot be longer than 255 characters.")]
    public string Badges { get; set; }
}