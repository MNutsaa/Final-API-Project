using Forum.Contracts;
using Forum.Data;
using Forum.Entities;
using Forum.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForumRepository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDbContext _context;

        private readonly DbSet<Comment> _dbComment;
        public CommentRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbComment = context.Set<Comment>();
        }
        public async Task AddCommentAsync(Comment entity)
        {
            if (entity != null)
            {
                await _context.Comments.AddAsync(entity);
            }
        }

        public async Task<int> CountAsync(Expression<Func<Comment, bool>> predicate)
        {
            return await _context.Comments
                .CountAsync(predicate);
        }

        public void DeleteComment(Comment entity)
        {
            if (entity != null)
            {
                _context.Comments.Remove(entity);
            }
        }

        public async Task<List<Comment>> GetAllCommentsAsync(Expression<Func<Comment, bool>> filter, string? includePropeties = null)
        {
            IQueryable<Comment> query = _dbComment;

            if (!string.IsNullOrWhiteSpace(includePropeties))
            {
                foreach (var includeProperty in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query
               .Where(filter)
               .ToListAsync();
        }

        public async Task<Comment?> GetSingleCommentAsync(Expression<Func<Comment, bool>> filter, string? includePropeties = null)
        {
            IQueryable<Comment> query = _dbComment;

            if (!string.IsNullOrWhiteSpace(includePropeties))
            {
                foreach (var includeProperty in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return await query.FirstOrDefaultAsync(filter);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<Comment> UpdateCommentAsync(Comment entity)
        {

            var commentToUpdate = await _context.Comments.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (commentToUpdate == null)
            {
                throw new CommentNotFoundException();
            }
            commentToUpdate.Content = entity.Content;

            _context.Comments.Update(commentToUpdate);
            return commentToUpdate;
        }
        public void RemoveRange(IEnumerable<Comment> entities)
        {
            _context.Set<Comment>().RemoveRange(entities);
        }

        public Task<int> CountAsync(Expression<Func<Comment, bool>> predicate, string? includePropeties = null)
        {
            IQueryable<Comment> query = _dbComment;

            if (!string.IsNullOrWhiteSpace(includePropeties))
            {
                foreach (var includeProperty in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.CountAsync();

        }
    }
}
