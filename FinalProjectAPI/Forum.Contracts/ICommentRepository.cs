using Forum.Entities;
using System.Linq.Expressions;

namespace Forum.Contracts
{
    public interface ICommentRepository : ISavable
    {
        Task<List<Comment>> GetAllCommentsAsync(Expression<Func<Comment, bool>> filter, string? includePropeties = null);
        Task<Comment> GetSingleCommentAsync(Expression<Func<Comment, bool>> filter, string? includePropeties = null);
        Task AddCommentAsync(Comment entity);
        Task<Comment> UpdateCommentAsync(Comment entity);
        void DeleteComment(Comment entity);
        void RemoveRange(IEnumerable<Comment> entity);
        Task<int> CountAsync(Expression<Func<Comment, bool>> predicate, string? includePropeties = null);
    }
}
