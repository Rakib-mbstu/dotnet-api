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

        public async Task<Comments> CreateAsync(Comments comments)
        {
            await _context.Comments.AddAsync(comments);
            await _context.SaveChangesAsync();
            return comments;
        }

        public async Task<Comments?> DeleteAsync(int id)
        {
            var comment = await _context.Comments.FirstOrDefaultAsync(x=>x.Id==id);
            if(comment == null)
            {
                return null;
            }
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return comment;
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

        public async Task<Comments?> UpdateAsync(int id, Comments comments)
        {
            var comment = await _context.Comments.FindAsync(id);
            if(comment == null)
            {
                return null;
            }
            comment.Title = comments.Title;
            comment.Content = comments.Content;
            await _context.SaveChangesAsync();
            return comment;
        }
    }
}