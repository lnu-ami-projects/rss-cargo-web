using System.ComponentModel.DataAnnotations;

namespace RSSCargo.PL.Models;

public class SignUpViewModel
{
    [Required(ErrorMessage = "* Email address is required.")]
    [EmailAddress(ErrorMessage = "* Invalid email address.")]
    public string? Email { get; init; }

    [Required(ErrorMessage = "* Username is required.")]
    public string? Username { get; init; }

    [Required(ErrorMessage = "* Password is required.")]
    [StringLength(20, MinimumLength = 8, ErrorMessage = "* Password must be between 8 and 20 characters.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[!@#$%^&*()_\-+=])[A-Za-z\d!@#$%^&*()_\-+=]{8,20}$", 
        ErrorMessage = "* Password must contain at least one letter, one digit, and one special character.")]
    public string? Password { get; init; }

    [Compare("Password", ErrorMessage = "* Passwords do not match.")]
    [Required(ErrorMessage = "* Confirm Password is required.")]
    public string? ConfirmPassword { get; init; }
}