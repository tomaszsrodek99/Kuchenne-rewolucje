using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Dtos
{
    public class CategoryWithDetailsView
    {
        public int Id { get; set; }

        [Display(Name = "Nazwa kategori")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Liczba przepisów w tej kategorii")]
        public int? NumberOfRecipes { get; set; }
    }
}
