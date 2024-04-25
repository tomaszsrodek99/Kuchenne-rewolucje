using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Kuchenne_rewolucje.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
        public string Username { get; set; }
        public int ArticleId { get; set; }
        [JsonIgnore]
        public Article? Article { get; set; }
        [Required]
        public string Content { get; set; }
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
