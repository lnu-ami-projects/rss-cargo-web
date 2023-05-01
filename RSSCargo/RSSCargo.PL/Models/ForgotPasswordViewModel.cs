using System.ComponentModel.DataAnnotations;

namespace RSSCargo.PL.Models
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
