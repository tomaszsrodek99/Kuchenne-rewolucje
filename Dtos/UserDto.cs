using Kuchenne_rewolucje.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuchenne_rewolucje.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2"), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        [Display(Name = "Nazwa użytkownika")]
        public string Username { get; set; } = string.Empty;
        [Column(TypeName = "varbinary(max)")]
        public byte[]? ImageUrl { get; set; }
        [Display(Name = "Obrazek profilowy"), Required(ErrorMessage = "Pole jest wymagane.")]
        public IFormFile? ImageFile { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public List<FavouritesArticle>? Favourites { get; set; }
    }
}
