using AutoMapper;
using Kuchenne_rewolucje.Context;
using Kuchenne_rewolucje.Dtos;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.EntityFrameworkCore;

namespace Kuchenne_rewolucje.Repositories
{

    public class CategoryRepository(MyDbContext context, IMapper mapper) : GenericRepository<Category>(context), ICategoryRepository
    {
        private readonly MyDbContext _context = context;
        private readonly IMapper _mapper = mapper;

        public async Task<bool> CategoryExists(string name)
        {
            var categories = await _context.Categories.ToListAsync();
            return categories.Any(c => string.Equals(c.Name, name, StringComparison.Ordinal));
        }



        public async Task<Category> GetCategoryByName(string name)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(u => u.Name == name);
            return category;
        }

        public async Task<List<CategoryDto>> GetCategories()
        {
            var categories = await GetAllAsync();

            if (categories.Count == 0)
                return [];

            var categoriesDto = _mapper.Map<List<CategoryDto>>(categories).OrderBy(x => x.Name).ToList();
            return categoriesDto;
        }

        public async Task<List<CategoryWithDetailsView>> GetCategoriesWithStats()
        {
            List<CategoryWithDetailsView> categoriesWithStats = await _context.Categories
        .Select(category => new CategoryWithDetailsView
        {
            Id = category.Id,
            Name = category.Name,
            NumberOfRecipes = category.Articles.Count()
        })
        .ToListAsync();

            if (categoriesWithStats.Count == 0)
                return [];

            return categoriesWithStats;
        }

        public override async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);


            bool isUsed = await _context.Articles.AnyAsync(p => p.CategoryId == id);

            if (isUsed)
            {
                throw new InvalidOperationException("Nie można usunąć tej kategorii, ponieważ jest już używana w przepisie.");
            }

            base.DeleteAsync(id);
        }
    }
}
