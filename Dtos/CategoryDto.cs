using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Dtos
{
    public class CategoryDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane."), MinLength(2, ErrorMessage = "Minmalna ilość znaków to 2"), MaxLength(128, ErrorMessage = "Maksymalna długość to 128 znaków.")]
        [Display(Name = "Nazwa kategori")]
        public string Name { get; set; } = string.Empty;
        public List<ArticleDto>? Articles { get; set; }
    }
}
