using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Dtos
{
    public class UserRegisterRequestDto
    {
        public int? Id { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Hasło")]
        [MinLength(6, ErrorMessage = "Minimalna długość to 6 znaków.")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane."), Compare("Password", ErrorMessage = "Hasła nie są identyczne.")]
        [Display(Name = "Potwierdż hasło")]
        public string ConfirmPassword { get; set; } = string.Empty;
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; } = string.Empty;
    }
}
