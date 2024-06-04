using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace WebApplication4.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters long.")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Surname is required.")]
    [StringLength(50, MinimumLength = 1, ErrorMessage = "Surname cannot be longer than 50 characters.")]
    public string UserSurname { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters long.")] 
    public string Password { get; set; }
    
    
    public string? ActivationToken { get; set; }
    public bool IsActive { get; set; } = false; // Domyślnie konto nieaktywne
    

    public virtual ICollection<Recipe> FavoriteRecipes { get; set; } = new List<Recipe>();
    public virtual ICollection<User> Friends { get; set; } = new List<User>();
    public virtual ICollection<FriendRequest> FriendRequests { get; set; } = new List<FriendRequest>();
    
    
    
    
}