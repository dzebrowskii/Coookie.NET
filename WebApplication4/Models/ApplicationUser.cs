using System.ComponentModel.DataAnnotations;
namespace WebApplication4.Models;

public class ApplicationUser
{
    [Key]
    public int UserId { get; set; }
    [Required(ErrorMessage = "Imię jest wymagane.")]
    
    [StringLength(50, ErrorMessage = "Imię nie może być dłuższe niż 50 znaków.")]
    public string UserName { get; set; }
    
    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [StringLength(50, ErrorMessage = "Nazwisko nie może być dłuższe niż 50 znaków.")]
    public string UserSurname { get; set; }
    
    [Required(ErrorMessage = "Adres email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu email.")]
    public string UserEmail { get; set; }
    
    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [DataType(DataType.Password)]
    public string UserPassword { get; set; }
    
    public virtual ICollection<ApplicationUser> Friends { get; set; }
    public virtual ICollection<Recipe> Favorite_Recipes{ get; set; }

    public ApplicationUser()
    {
        Friends = new HashSet<ApplicationUser>();
    }
    // DO Poprawy jak zainicjalizować te 2 listy ?
    public Favorite_Recipes()
    {
        Favorite_Recipes = new HashSet<Recipe>();
    }
    
    
}