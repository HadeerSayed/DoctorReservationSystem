using final_project.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace models
{
	public class Admin
    {
        [Key]
        public int ID { get; set; }
        [Required]
        [MaxLength(450,ErrorMessage ="max length is 450 char")]
        [ForeignKey("user")]
        public string userId { get; set; }
        public ApplicationUser? user { get; set; }
    }
}
