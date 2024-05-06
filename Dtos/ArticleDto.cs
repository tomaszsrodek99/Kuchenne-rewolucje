using Kuchenne_rewolucje.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kuchenne_rewolucje.Dtos
{
    public class ArticleDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2"), MaxLength(64, ErrorMessage = "Maksymalna długość to 64 znaków.")]
        [Display(Name = "Nazwa dania")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public UserDto User { get; set; }

        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        [Display(Name = "Kategoria dania")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2")]
        [Display(Name = "Składniki")]
        public string Ingredients { get; set; }
        public Category Category { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2")]
        [Display(Name = "Przygotowanie")]
        public string Content { get; set; }
        [Column(TypeName = "varbinary(max)")]
        public string? ImageUrl { get; set; }
        [Display(Name = "Zdjęcie")]
        public IFormFile? ImageFile { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public List<FavouritesArticle>? Favourites { get; set; }
        public double? Rating { get; set; }
    }
}
