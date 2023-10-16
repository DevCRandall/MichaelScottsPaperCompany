using System.ComponentModel.DataAnnotations;

namespace MichaelScottsPaperCompany.Models
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "First Name is required")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "An Email is required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please include a Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Please include a Message is required")]
        [StringLength(2000)]
        public string Message { get; set; }

    }
}
