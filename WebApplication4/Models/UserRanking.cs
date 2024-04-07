using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class UserRanking
{
    [Key]
    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    [Required]
    public int RankPoints { get; set; }

    public string Badges { get; set; }
}