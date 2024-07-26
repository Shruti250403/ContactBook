using System.ComponentModel.DataAnnotations;

namespace ClientPhoneBookApp.ViewModels
{
    public class StateViewModel
    {
        public int StateId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
    }
}
