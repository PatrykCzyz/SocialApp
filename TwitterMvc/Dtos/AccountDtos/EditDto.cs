using System.ComponentModel.DataAnnotations;
using TwitterMvc.Enums;

namespace TwitterMvc.Dtos.AccountDtos
{
    public class EditDto
    {
        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Lastname { get; set; }

        [Required]
        [Range(13, 100)]
        public int? Age { get; set; }

        [Required]
        public GenderEnum Gender { get; set; }

        [Required]
        public string Country { get; set; }
    }
}