using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Models;

namespace Kuchenne_rewolucje.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<bool> CategoryExists(string name);
        Task<Category> GetCategoryByName(string name);
        Task<List<CategoryDto>> GetCategories();
        Task<List<CategoryWithDetailsView>> GetCategoriesWithStats();
    }
}
