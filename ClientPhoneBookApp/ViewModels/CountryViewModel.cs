using System.ComponentModel.DataAnnotations;

namespace ClientPhoneBookApp.ViewModels
{
    public class CountryViewModel
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        public string CountryName { get; set; }
    }
}
