using Forum.Contracts;
using Forum.Data;
using Forum.Entities;
using Forum.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ForumRepository
{
    public class TopicRepository : ITopicReporitory
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<Topic> _dbTopics;
        public TopicRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbTopics = context.Set<Topic>();
        }
        public async Task AddTopicAsync(Topic entity)
        {
            if (entity != null)
            {
                await _context.Topics.AddAsync(entity);
            }
        }

        public void DeleteTopic(Topic entity)
        {
            if (entity != null)
            {
                _context.Topics.Remove(entity);
            }
        }


        public async Task<List<Topic>> GetAllTopicsAsync(string? includePropeties = null)
        {
            IQueryable<Topic> query = _dbTopics;

            if (!string.IsNullOrWhiteSpace(includePropeties))
            {
                foreach (var includeProperty in includePropeties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            var topics = await query.ToListAsync();

            return topics;
        }

        public async Task<List<Topic>> GetAllTopicsAsync(Expression<Func<Topic, bool>> filter, string? includePropeties = null)
        {
            IQueryable<Topic> query = _dbTopics;

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

        public async Task<Topic?> GetSingleTopicAsync(Expression<Func<Topic, bool>> filter, string? includePropeties = null)
        {
            IQueryable<Topic> query = _dbTopics;

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

        public async Task<Topic> UpdateTopicAsync(Topic entity)
        {
            var topicToUpdate = await _context.Topics.FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (topicToUpdate == null)
            {
                throw new TopicNotFoundException();
            }
            topicToUpdate.Title = entity.Title;
            topicToUpdate.Description = entity.Description;
            topicToUpdate.State = entity.State;
            topicToUpdate.Status = entity.Status;

            _context.Topics.Update(topicToUpdate);
            return topicToUpdate;

        }
    }
}
