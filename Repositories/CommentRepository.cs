using AutoMapper;
using Kuchenne_rewolucje.Context;
using Kuchenne_rewolucje.Interfaces;
using Kuchenne_rewolucje.Models;
using Microsoft.EntityFrameworkCore;

namespace Kuchenne_rewolucje.Repositories
{

    public class CommentRepository(MyDbContext context) : GenericRepository<Comment>(context), ICommentRepository
    {
        private readonly MyDbContext _context = context;

        public override async Task<Comment> GetAsync(int? id)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
