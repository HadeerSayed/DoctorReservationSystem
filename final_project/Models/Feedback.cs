using System.ComponentModel.DataAnnotations;

namespace models
{
	public class Feedback
	{
		[Key]
		public int ID { get; set; }
		[MaxLength(1)]
		[RegularExpression("^[1 5]{1}$", ErrorMessage = "Rate Should be from one To five ")]
		public string? Rate { get; set; }
		[Required]
		[MaxLength(500, ErrorMessage = "Maximum Letters is 500 ")]
        public string Comment { get; set; }
		public int? DoctorId { get; set; }
		public Doctor Doctor { get; set; }
		public int? PatientId { get; set; }
		public Patient patient { get; set; }

	}
}
