namespace WebApplication4.Models;
using System.ComponentModel.DataAnnotations;

public class AppRating
{

    [Key] 
    public int Id { get; set; }
    
    [Required]
    public int Value { get; set; }
    
    public string UserId { get; set; }
    
    
}