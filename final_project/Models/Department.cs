using System.ComponentModel.DataAnnotations;

namespace models
{
	public class Department
	{
		[Key]
		public int ID { get; set; }
		[Required]
		[MaxLength(50)]
		[RegularExpression("^[a-z A-Z]{3,}$", ErrorMessage = "Name Should be letters only and greater than 2")]
		public string Name { get; set; }
		public virtual List<Doctor> Doctors { get;set; }=new List<Doctor>();
	}
}
