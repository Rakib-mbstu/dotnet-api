using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentsRepository(ApplicationDBContext context)
        {
            _context = context;   
        }
        public async Task<List<Comments>> GetAllAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<Comments?> GetByIdAsync(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment == null)
            {
                return null;
            }
            return comment;
        }
        
    }
}