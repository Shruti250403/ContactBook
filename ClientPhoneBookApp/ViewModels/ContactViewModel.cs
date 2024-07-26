using APIPhoneBook.Models;
using System.ComponentModel.DataAnnotations;

namespace ClientPhoneBookApp.ViewModels
{
    public class ContactViewModel
    {
        [Key]
        public int ContactId { get; set; }

        [Required]
        [StringLength(15)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(15)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string Phone { get; set; }

        [Required]
        [StringLength(15)]
        public string Company { get; set; }

        public string? Image { get; set; }
        public byte[]? ImageByte { get; set; }
        public IFormFile file { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Favourites is required.")]
        public bool Favourites { get; set; }
        [Required(ErrorMessage = "Country is required.")]
        public int CountryId { get; set; }
        [Required(ErrorMessage = "State is required.")]
        public int StateId { get; set; }
        public StateViewModel? State { get; set; }
        public DateTime? BirthDate { get; set; }

        public CountryViewModel? Country { get; set; }
        public List<StateViewModel>? States { get; set; }
        public List<CountryViewModel>? Countries { get; set; }
        //public Country Country { get; set; }
        //public State State { get; set; }
    }
}
