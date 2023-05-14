namespace RSSCargo.PL.Models;

using System.ComponentModel.DataAnnotations;

public class SignInViewModel
{
    [Required(ErrorMessage = "* Email address is required.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "* Password is required.")]
    public string Password { get; set; }
}
