using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Covalence.ViewModels
{
    public class UserViewModel
    {
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }
        public bool? ShareLocation { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public bool? NeedsOnboarding { get; set; }
    }
}