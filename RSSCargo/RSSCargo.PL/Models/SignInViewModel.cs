using System.ComponentModel.DataAnnotations;

namespace RSSCargo.PL.Models;

public class SignInViewModel
{
    [Required(ErrorMessage = "* Email address is required.")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "* Password is required.")]
    public string? Password { get; set; }
}