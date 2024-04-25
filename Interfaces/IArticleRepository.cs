using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Interfaces
{
    public interface IArticleRepository : IGenericRepository<Article>
    {
        Task<List<ArticleDto>> GetArticles(int? id);
        Task<bool> ArticleExists(string name);
        Task<List<CategoryDto>> GetArticlesByUserId(int id);
        Task<int> GetNumberOfArticlesForUser(int id);
        Task<int> GetNumberOfCommentsForUser(int id);
        Task<int> GetNumberOfRatingsForUser(int id);
    }
}
