using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Dtos
{
    public class UserLoginRequest
    {
        [MaxLength(128, ErrorMessage = "Maksymalna długośc to 128 znaków."), Required(ErrorMessage = "Pole jest wymagane.")]
        public string Username { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Hasło")]
        public string Password { get; set; } = string.Empty;
    }
}
