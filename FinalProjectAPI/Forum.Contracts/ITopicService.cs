using Forum.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Forum.Contracts
{
    public interface ITopicService
    {
        Task<List<TopicGettingDto>> GetAllTopicsAsync();
        Task<List<TopicGettingDto>> GetTopicsOfUserAsync(string userId);
        Task<TopicGettingDto> GetSingleTopicByUserId(string topicId, string userId);
        Task UpdateStateAsync(string id, JsonPatchDocument<StateUpdatingDto> patchDocument);
        Task DeleteTopicAsync(string id);
        Task AddTopicAsync(TopicCreatingDto topicCreatingDto);
        Task UpdateTopicAsync(string topicId, JsonPatchDocument<TopicUpdatingDto> patchDocument);
    }
}
