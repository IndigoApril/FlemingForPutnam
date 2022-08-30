using System.ComponentModel.DataAnnotations;

namespace electFleming.ViewModels
{
    public class ContactFormViewModel
    {
        [Required(ErrorMessage = "Please enter your name")]
        [Display(Name = "Your Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter a subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please enter your email address")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(500, ErrorMessage = "Your message must be 500 characters or less")]
        public string Message { get; set; }

        [MaxLength(15, ErrorMessage = "Your phone number must be 15 characters or less")]

        [Display(Name = "Phone #")]
        public string PhoneNumber { get; set; }
    }
}
