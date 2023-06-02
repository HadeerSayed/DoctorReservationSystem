using customvalidtion;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace models
{
	public class Appointment
	{
		[Key]
		public int ID { get; set; }
		[Required]
		[BindProperty, DataType(DataType.Date)]
        [appointmentdatevalidation]
        public DateTime Date { get; set; }
		[Required]
        [RegularExpression("^((0[0-9]{1})|(1[0-9]{1})|(2[0-4]{1})):(([0-5]{1}[0-9]{1})|60)$", ErrorMessage = "time must be in format[HH:MM]")]
        public string Time { get; set; }
		public bool State { get; set; }
		public bool completed { get; set; }
		public int? DoctorId { get; set; }
		public Doctor Doctor { get; set; }
		public int? PatientId { get; set; }
		public Patient Patient { get; set; }
		public int schaduleId { get; set; }
		public Schadule schadule { get; set; }

	}
}
