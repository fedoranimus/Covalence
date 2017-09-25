using System.ComponentModel.DataAnnotations;

namespace Covalence.ViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}