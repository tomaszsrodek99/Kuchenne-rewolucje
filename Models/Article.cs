using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
        public int CategoryId { get; set; }
        public string Ingredients { get; set; }
        public string Content { get; set; }
        public string? ImageUrl { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<FavouritesArticle>? Favourites { get; set; }
        public List<Rating>? Ratings { get; set; }
    }
}
