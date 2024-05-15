using AutoMapper;
using Kuchenne_rewolucje.Context;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.EntityFrameworkCore;

namespace Kuchenne_rewolucje.Repositories
{
    public class ArticleRepository(MyDbContext context, IMapper mapper) : GenericRepository<Article>(context), IArticleRepository
    {
        private readonly MyDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<List<ArticleDto>> GetArticles(int? id)
        {
            var articles = await _context.Articles
                .Include(x => x.User)
                .Include(f => f.Favourites)
                .ToListAsync();


            if (articles.Count == 0)
                return [];

            if (id != null)
                articles = articles.Where(x => x.CategoryId == id).ToList();

            var articlesDto = _mapper.Map<List<ArticleDto>>(articles).OrderBy(x => x.Name).ToList();
            return articlesDto;
        }

        public async Task<List<CategoryDto>> GetArticlesByUserId(int id)
        {
            var articles = await _context.Categories.Include(x => x.Articles.Where(a => a.UserId == id).OrderBy(a => a.Name)).OrderBy(x => x.Name).ToListAsync();

            if (articles.Count == 0)
                return [];

            return _mapper.Map<List<CategoryDto>>(articles);
        }

        public async Task<int> GetNumberOfArticlesForUser(int id)
        {
            var userArticles = await _context.Articles.Where(c => c.UserId == id).ToListAsync();
            return userArticles.Count;
        }

        public async Task<int> GetNumberOfCommentsForUser(int id)
        {
            var userComments = await _context.Comments.Where(c => c.UserId == id).ToListAsync();
            return userComments.Count;
        }
        public async Task<int> GetNumberOfRatingsForUser(int id)
        {
            var userRatings = await _context.Ratings.Where(c => c.UserId == id).ToListAsync();
            return userRatings.Count;
        }

        public async Task<bool> ArticleExists(string name)
        {
            var article = await _context.Articles.FirstOrDefaultAsync(u => u.Name == name);
            if (article == null)
                return false;

            return true;
        }

        public override async Task<Article> GetAsync(int? id)
        {
            var article = await _context.Articles.Where(x => x.Id == id)
                .Include(x => x.Comments.OrderBy(x => x.CreatedAt))
                .Include(x => x.Ratings)
                .Include(x => x.User)
                .FirstOrDefaultAsync();

            return article;
        }
    }
}
