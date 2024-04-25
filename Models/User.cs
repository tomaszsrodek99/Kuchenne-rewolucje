using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public required string Username { get; set; }
        public byte[]? ImageUrl { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<FavouritesArticle>? Favourites { get; set; }
        public List<Rating>? Ratings { get; set; }
    }
}
