using System.ComponentModel.DataAnnotations;

namespace WebApplication4.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
    public string Username { get; set; }

    [Required]
    [StringLength(50, ErrorMessage = "Surname cannot be longer than 50 characters.")]
    public string UserSurname { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    public virtual ICollection<Recipe> FavoriteRecipes { get; set; }
    public virtual ICollection<User> Friends { get; set; }
}