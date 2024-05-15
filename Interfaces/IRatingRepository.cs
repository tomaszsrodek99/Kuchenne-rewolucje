using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Interfaces
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<Rating> GetUserRate(int userId, int articleId);
        Task<Rating> GetRating(int userId, int articleId);
    }
}
