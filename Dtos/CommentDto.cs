using System.ComponentModel.DataAnnotations;

namespace Kuchenne_rewolucje.Dtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public int ArticleId { get; set; }
        public ArticleDto? Article { get; set; }
        [Required(ErrorMessage = "Pole jest wymagane.")]
        public string Content { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }
    }
}
