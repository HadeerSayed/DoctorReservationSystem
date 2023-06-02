using final_project.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace models
{
	public class Patient 
    {
		[Key]
		public int ID { get; set; }
		[Required]
		[MaxLength(450)]
		[ForeignKey("user")]
		public string userId { get; set; }
		public ApplicationUser? user { get; set; }
		public virtual List<Feedback> feedbacks { get; set; } = new List<Feedback>();
		public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();
	}
}
