using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TI_Net2025_DemoCleanAsp.Models.Users
{
    public class RegisterFormDto
    {
        [Required]
        [MaxLength(150)]
        [DisplayName("Email")]
        public string Email { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Mot de passe")]
        public string Password { get; set; } = null!;

        [Required]
        [Compare("Password",ErrorMessage = "Les mots de passes doivent correspondre")]
        [DataType(DataType.Password)]
        [DisplayName("Mot de passe de confirmation")]
        public string ConfirmPassword { get; set; } = null!;
    }
}
