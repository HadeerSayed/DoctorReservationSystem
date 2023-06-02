using final_project.Models;
using System.ComponentModel.DataAnnotations;

namespace models
{
	public class Clinic
	{
		[Key]
		public int ID { get; set; }
		[Required]
		[MaxLength(100)]
		[RegularExpression("^[a-z A-Z 0-9]{8,}$", ErrorMessage = "Name Should be letters with numbers only and greater than 7")]
		public string Name { get; set; }
		[Required]
		[MaxLength(11)]
		[RegularExpression("^(010|012|011|015)[0-9]{8}$", ErrorMessage = "Enter Correct Phone Number")]
		public string Phone { get; set; }
		[Required]
		[MaxLength(150,ErrorMessage ="max length is 150 char")]
		[RegularExpression("^[a-z A-Z 0-9]{8,}$", ErrorMessage = "Address Should be letters with numbers only and greater than 7")]
		public string Address { get; set; }
		[MaxLength(20)]
		[RegularExpression("^((\\-?|\\+?)?(((([0-9])|([1-8][0-9]))(\\.[0-9]{4,}))|(90\\.[0]{4,})),(\\-?|\\+?)?(((([0-9])|([1-9][0-9])|(1[0-7][0-9]))(\\.[0-9]{4,}))|(180\\.[0]{4,})))$", ErrorMessage = "Enter Correct Number")]
		public string? Longitude { get; set; }
		public decimal cost { get; set; }
		public int? DoctorId { get; set; }
		public Doctor Doctor { get; set; }
        public int countryid { get; set; }
        public country country { get; set; }
        public virtual List<Schadule> Schadules { get;set; }= new List<Schadule>();


	}
}
