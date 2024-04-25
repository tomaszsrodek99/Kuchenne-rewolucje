using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Interfaces
{
    public interface IRatingRepository : IGenericRepository<Rating>
    {
        Task<double> GetUserRate(int userId, int articleId);
    }
}
