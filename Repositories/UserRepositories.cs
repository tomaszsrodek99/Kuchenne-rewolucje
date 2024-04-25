using AutoMapper;
using Kuchenne_rewolucje.Context;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Kuchenne_rewolucje.Repositories
{

    public class UserRepository(MyDbContext context, IMapper mapper) : GenericRepository<User>(context), IUserRepository
    {
        private readonly MyDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> UserExists(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
                return false;

            return true;
        }

        public async Task<User> GetUserByEmail(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            return user;
        }

        public async Task<List<UserDto>> GetUsers()
        {
            var users = await GetAllAsync();

            if (users.Count == 0)
                return [] ;

            var usersDto = _mapper.Map<List<UserDto>>(users).OrderBy(x => x.Username).ToList();
            return usersDto;
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        public override async Task<User> GetAsync(int? id)
        {
            var user = await _context.Users.Include(x=>x.Favourites.Where(u => u.UserId == id)).FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
    }
}
