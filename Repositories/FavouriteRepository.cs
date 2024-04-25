using AutoMapper;
using Kuchenne_rewolucje.Context;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.EntityFrameworkCore;

namespace Kuchenne_rewolucje.Repositories
{

    public class FavouriteRepository(MyDbContext context) : GenericRepository<FavouritesArticle>(context), IFavouriteRepository
    {
        private readonly MyDbContext _context = context;

        public override async Task<FavouritesArticle> GetAsync(int? id)
        {
            return await _context.Favourites.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
