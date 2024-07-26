using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ClientContactPhonebook.ViewModels
{
    public class AddContactViewModel
    {
        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(50)]
        [EmailAddress]
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        [DisplayName("Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(15)]
        public string Company { get; set; }

        public string? Image { get; set; }
    }
}
