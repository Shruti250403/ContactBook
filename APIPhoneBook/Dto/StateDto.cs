using APIPhoneBook.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Dto
{
    [ExcludeFromCodeCoverage]
    public class StateDto
    {
        public int StateId { get; set; }
        [Required(ErrorMessage = "State name is required.")]
        public string StateName { get; set; }
        public int CountryId { get; set; }
        public Country Country { get; set; }
    }
}
