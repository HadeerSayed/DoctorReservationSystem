using models;
using System.ComponentModel.DataAnnotations;

namespace models
{
    public class country
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        [RegularExpression("^[a-z A-Z]{3,}$", ErrorMessage = "Name Should be letters only and greater than 2")]
        public string Name { get; set; }
        public virtual List<Clinic> Clinics { get; set; }=new List<Clinic>();
    }
}
