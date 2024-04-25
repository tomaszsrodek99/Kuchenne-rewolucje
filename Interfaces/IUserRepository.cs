using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> UserExists(string username);
        Task<User> GetUserByEmail(string username);
        Task<List<UserDto>> GetUsers();
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    }
}
