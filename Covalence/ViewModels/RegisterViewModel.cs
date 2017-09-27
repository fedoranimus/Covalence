using System.ComponentModel.DataAnnotations;

namespace Covalence.ViewModels
{
    public class RegisterViewModel
    {
        [RequiredAttribute]
        [EmailAddressAttribute]
        [DisplayAttribute(Name = "Email")]
        public string Email { get; set; }
        
        [RequiredAttribute]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataTypeAttribute(DataType.Password)]
        [DisplayAttribute(Name = "Password")]
        public string Password { get; set; }

        [DisplayAttribute(Name = "First Name")]
        public string FirstName { get; set; }
        [DisplayAttribute(Name = "Last Name")]
        public string LastName { get; set;}
        [DisplayAttribute(Name = "Location")]
        public string Location { get; set; }
    }
}