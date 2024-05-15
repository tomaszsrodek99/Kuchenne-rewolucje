using AutoMapper;
using Kuchenne_rewolucje.Context;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.EntityFrameworkCore;

namespace Kuchenne_rewolucje.Repositories
{

    public class RatingRepository(MyDbContext context) : GenericRepository<Rating>(context), IRatingRepository
    {
        private readonly MyDbContext _context = context;

        public override async Task<Rating> GetAsync(int? id)
        {
            return await _context.Ratings.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<double> GetUserRate(int userId, int articleId)
        {
            var value = await _context.Ratings.Where(x =>x.UserId == userId && x.ArticleId == articleId).Select(x=>x.Value).FirstOrDefaultAsync();

            if (value == null)
                return 0;

            return value;
        }
    }
}
