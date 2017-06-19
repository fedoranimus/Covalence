using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Covalence.ViewModels {
    public class PostViewModel {
        [Required]
        [StringLength(500, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        [Required]
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Required]
        [Display(Name = "Tags")]
        public List<string> Tags { get; set; }
        [Required]
        [Display(Name = "Category")]
        public int Category { get; set; }
    }
}