using models;
using System.ComponentModel.DataAnnotations;

namespace models
{
	public class searchresult
	{
		public int ID { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public string Address { get; set; }
		public decimal cost { get; set; }
		public int? DoctorId { get; set; }
		public string Title { get; set; }
		public string country { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Department { get; set; }
		public string image { get; set; }
		public gender gender { get; set; }


	}
}
