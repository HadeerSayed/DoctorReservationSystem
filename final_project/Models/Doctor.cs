using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using final_project.Models;
using Microsoft.AspNetCore.Http;
namespace models
{
	public class Doctor
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(450)]
        [ForeignKey("user")]
        public string userId { get; set; }
        public ApplicationUser? user { get; set; }
        [MaxLength(200)]
		public string? Image { get; set; }
        [NotMapped]
        public IFormFile? imageFile { get; set; }
		[MaxLength(500)]
		public string? about { get; set; }
        public int? DepartmentId { get; set; }
		public Department Department { get; set; }
        public int? globalrate { get; set; }
        public int? titleid { get; set; }
        public virtual title Title { get; set; }
		public virtual List<Clinic> Clinics { get; set; } = new List<Clinic>();
		public virtual List<Appointment> Appointments { get; set; } = new List<Appointment>();
		public virtual List<Feedback> feedbacks { get; set; } = new List<Feedback>();

	}
}
