using System.ComponentModel.DataAnnotations;

namespace models
{
    public class ChangePassword
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }

        [Required]
        [MaxLength(25)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

    }
}
