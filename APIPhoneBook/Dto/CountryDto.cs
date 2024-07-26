using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace APIPhoneBook.Dto
{
    [ExcludeFromCodeCoverage]
    public class CountryDto
    {
        public int CountryId { get; set; }
        [Required(ErrorMessage = "Country name is required.")]
        public string CountryName { get; set; }
    }
}
