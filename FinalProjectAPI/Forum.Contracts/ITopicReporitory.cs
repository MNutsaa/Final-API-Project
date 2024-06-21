using Forum.Entities;
using System.Linq.Expressions;

namespace Forum.Contracts
{
    public interface ITopicReporitory : ISavable
    {
        Task<List<Topic>> GetAllTopicsAsync(string? includePropeties = null);
        Task<List<Topic>> GetAllTopicsAsync(Expression<Func<Topic, bool>> filter, string? includePropeties = null);
        Task<Topic> GetSingleTopicAsync(Expression<Func<Topic, bool>> filter, string? includePropeties = null);
        Task AddTopicAsync(Topic entity);
        Task<Topic> UpdateTopicAsync(Topic entity);
        void DeleteTopic(Topic entity);
    }
}
