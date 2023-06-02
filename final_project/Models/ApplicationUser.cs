using Microsoft.AspNetCore.Identity;
using models;
using System.ComponentModel.DataAnnotations;

namespace final_project.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        [MaxLength(50, ErrorMessage = "The Maximun Length Is 50")]
        [RegularExpression("^[a-z A-Z]{3,}$", ErrorMessage = "Name Should be letters only and greater than 2")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "The Maximun Length Is 50")]
        [RegularExpression("^[a-z A-Z]{3,}$", ErrorMessage = "Name Should be letters only and greater than 2")]
        public string LastName { get; set; }
        [Required]
        [MaxLength(11, ErrorMessage = "The Maximun Length Is 11")]
        [RegularExpression("^(010|012|011|015)[0-9]{8}$", ErrorMessage = "Enter Correct Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [EnumDataType(typeof(gender))]
        public gender Gender { get; set; }

	}
}
