using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Dtos
{
    public class UserLoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
