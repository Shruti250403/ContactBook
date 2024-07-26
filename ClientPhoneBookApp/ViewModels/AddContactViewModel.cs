using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ClientPhoneBookApp.ViewModels
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
        public string? Image { get; set; } = string.Empty;
        public IFormFile? file {  get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Favourites is required.")]
        public bool Favourites { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public DateTime? BirthDate { get; set; }
        public byte[]? ImageByte { get; set; }

        public List<StateViewModel>? States { get; set; }
        public List<CountryViewModel>? Countries { get; set; }
    }
}
