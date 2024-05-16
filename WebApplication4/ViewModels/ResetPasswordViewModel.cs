namespace WebApplication4.ViewModels;
using System.ComponentModel.DataAnnotations;
public class ResetPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }
}