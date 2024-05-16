namespace WebApplication4.ViewModels;
using System.ComponentModel.DataAnnotations;

public class ChangePasswordForLoggedInUserViewModel
{
    [Required(ErrorMessage = "Current Password is required")]
    
    public string CurrentPassword { get; set; }

    [Required(ErrorMessage = "New Password is required")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
    
    public string NewPassword { get; set; }

    [Required(ErrorMessage = "Confirm New Password is required")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; }
}