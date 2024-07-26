using System.ComponentModel.DataAnnotations;

namespace APIPhoneBook.Models
{
    public class State
    {
        [Key]
        public int StateId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public ICollection<ContactModel> Contacts { get; set; }
    }
}
