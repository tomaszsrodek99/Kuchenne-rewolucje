using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Kuchenne_rewolucje.Models
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }
        [DataType(DataType.Currency)]
        public double Value { get; set; }
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int ArticleId { get; set; }
        [JsonIgnore]
        public Article Article { get; set; }
    }
}
