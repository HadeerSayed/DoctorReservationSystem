using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace models
{
    public class DoctorClinic
    {

        [Key]
        public int DoctorID { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-z A-Z 0-9]{3,}$")]
        public string Username { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-z A-Z]{3,}$")]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        [RegularExpression("^[a-z A-Z]{3,}$")]
        public string LastName { get; set; }
        [MaxLength(500)]
        public string? about { get; set; }
        public int? titleid { get; set; }
        public int countryid { get; set; }
        public string? Image { get; set; }
        [NotMapped]
        public IFormFile? imageFile { get; set; }
        [Required]
        [EnumDataType(typeof(gender))]
        public gender Gender { get; set; }
        [Required]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [MaxLength(11)]
        [RegularExpression("^(010|012|011|015)[0-9]{8}$")]
        public string DoctorPhone { get; set; }
        public int? DepartmentId { get; set; }


        [Required]
        [MaxLength(100)]
        [RegularExpression("^[a-z A-Z 0-9]{8,}$")]
        public string ClinicName { get; set; }
        [Required]
        [MaxLength(11)]
        [RegularExpression("^(010|012|011|015)[0-9]{8}$")]
        public string ClinicPhone { get; set; }
        [Required]
        [MaxLength(150)]
        [RegularExpression("^[a-z A-Z 0-9]{8,}$")]
        public string ClinicAddress { get; set; }
        [MaxLength(20)]
        [RegularExpression("^((\\-?|\\+?)?(((([0-9])|([1-8][0-9]))(\\.[0-9]{4,}))|(90\\.[0]{4,})),(\\-?|\\+?)?(((([0-9])|([1-9][0-9])|(1[0-7][0-9]))(\\.[0-9]{4,}))|(180\\.[0]{4,})))$")]
        public string? Longitude { get; set; }

        public decimal cost { get; set; }

    }
}
