using models;
using System.ComponentModel.DataAnnotations;

namespace models
{
    public class title
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(150)]
        [RegularExpression("^[a-z A-Z]{3,}$", ErrorMessage = "Title Should be letters only and greater than 2")]
        public string Title { get; set; }
        public virtual List<Doctor> Doctors { get; set; }=new List<Doctor>();
    }
}
