using customvalidtion;
using System.ComponentModel.DataAnnotations;

namespace models
{
	public class Schadule
	{
        [Key]
        public int ID { get; set; }
		[Required]
        [MaxLength(5)]
        [RegularExpression("^((0[1-9]{1})|(1[0-9]{1})|(2[0-4]{1})):(([0-5]{1}[0-9]{1})|60)$",ErrorMessage="time must be in format[HH:MM]")]
        public string starttime { get; set; }
        [Required]
        [MaxLength(5)]
        [RegularExpression("^((0[1-9]{1})|(1[0-9]{1})|(2[0-4]{1})):(([0-5]{1}[0-9]{1})|60)$", ErrorMessage = "time must be in format[HH:MM]")]
        public string endtime { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [appointmentdatevalidation]
        public DateTime date { get; set; }
        [Required]
        [MaxLength(2)]
        [RegularExpression("^(([1-5]{1}[0-9]{1})|60)$", ErrorMessage = "time must be in format[MM]")]
        public string duration { get; set; }
        public int ClinicId { get; set; }
        public virtual Clinic Clinic { get; set; }
        public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();

    }
}
