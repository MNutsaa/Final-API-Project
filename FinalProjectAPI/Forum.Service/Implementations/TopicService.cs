using AutoMapper;
using Forum.Contracts;
using Forum.Entities;
using Forum.Models;
using Forum.Service.Exceptions;
using Microsoft.AspNetCore.JsonPatch;

namespace Forum.Service.Implementations
{
    public class TopicService : ITopicService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly ITopicReporitory _topicRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;


        public TopicService(ITopicReporitory topicRepository, MappingInitializer mappingInitializer, ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _topicRepository = topicRepository;
            _mapper = mappingInitializer.Initialize();
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }

        public async Task<List<TopicGettingDto>> GetAllTopicsAsync()
        {
            var topics = await _topicRepository.GetAllTopicsAsync(includePropeties: "Comments");
            return _mapper.Map<List<TopicGettingDto>>(topics);
        }

        public async Task AddTopicAsync(TopicCreatingDto topicCreatingDto)
        {
            var result = _mapper.Map<Topic>(topicCreatingDto);
            var user = _userRepository.GetAuthenticatedUserId();

            if (user is null)
            {
                throw new UserNotFoundExcpetion();
            }

            result.UserId = user;
            await _topicRepository.AddTopicAsync(result);
            await _topicRepository.Save();
        }

        public async Task DeleteTopicAsync(string id)
        {
            if (id == string.Empty)
                throw new ArgumentException("Invalid argument passed");

            var rawTopic = await _topicRepository.GetSingleTopicAsync(x => x.Id == id);

            if (rawTopic == null)
                throw new TopicNotFoundException();

            var userId = _userRepository.GetAuthenticatedUserId();
            var userRole = _userRepository.GetAuthenticatedUserRole().Trim();


            if (rawTopic.UserId.Trim() != userId && userRole != "Admin")
            {
                throw new UnauthorizedAccessException("Can't delete different userds Topic");

            }

            if (rawTopic.Comments != null)
            {
                _commentRepository.RemoveRange(rawTopic.Comments);
                await _commentRepository.Save();
            }
            _topicRepository.DeleteTopic(rawTopic);
            await _topicRepository.Save();
        }

        public async Task<TopicGettingDto> GetSingleTopicByUserId(string topicId, string userId)
        {

            if (topicId == string.Empty || string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Invalid argument passed");

            var rawTopic = await _topicRepository.GetSingleTopicAsync(x => x.Id == topicId && x.UserId == userId, includePropeties: "Comments");

            if (rawTopic == null || rawTopic.State != Forum.Entities.State.Show)
                throw new TopicNotFoundException();

            var result = _mapper.Map<TopicGettingDto>(rawTopic);
            return result;
        }
        public async Task<List<TopicGettingDto>> GetTopicsOfUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Invalid argument passed");

            var rawTopics = await _topicRepository.GetAllTopicsAsync(x => x.UserId.Trim() == userId.Trim(), includePropeties: "Comments");

            if (rawTopics == null || rawTopics.Count == 0)
            {
                throw new TopicNotFoundException();
            }

            var filteredRaw = rawTopics.Where(t => t.State == Forum.Entities.State.Show).ToList();

            if (filteredRaw.Count == 0)
            {
                throw new TopicNotFoundException();
            }
            var result = _mapper.Map<List<TopicGettingDto>>(filteredRaw);

            return result;
        }
        public async Task UpdateTopicAsync(string topicId, JsonPatchDocument<TopicUpdatingDto> patchDocument)
        {
            if (patchDocument is null)
            {
                throw new ArgumentNullException("Invalid argument passed");
            }

            if (topicId == string.Empty)
                throw new ArgumentException("Invalid argument passed");

            var userId = _userRepository.GetAuthenticatedUserId();
            if (userId is null)
            {
                throw new UnauthorizedAccessException("Must be logged in to update topic");
            }

            var rawTopic = await _topicRepository.GetSingleTopicAsync(x => x.Id == topicId);

            if (rawTopic == null)
                throw new TopicNotFoundException();
            if (rawTopic.Status == false)
                throw new TopicNotFoundException();
            if (rawTopic.UserId != userId)
                throw new UserNotFoundExcpetion();

            var topicToPatch = _mapper.Map<TopicUpdatingDto>(rawTopic);

            patchDocument.ApplyTo(topicToPatch);
            _mapper.Map(topicToPatch, rawTopic);

            await _topicRepository.Save();
        }

        public async Task UpdateStateAsync(string id, JsonPatchDocument<StateUpdatingDto> patchDocument)
        {
            if (patchDocument is null)
            {
                throw new ArgumentNullException("Invalid argument passed");
            }
            if (id == string.Empty)
            {
                throw new ArgumentException("Invalid argument passed");
            }

            byte show = 2;
            byte hide = 3;

            if (patchDocument.Operations.Any(op => op.value.ToString() != show.ToString() && op.value.ToString() != hide.ToString()))
            {
                throw new ArgumentException("Invalid argument passed");
            }

            var userId = _userRepository.GetAuthenticatedUserId();
            if (userId is null)
            {
                throw new UnauthorizedAccessException("Must be logged in to update topic");
            }
            var topicFromDb = await _topicRepository.GetSingleTopicAsync(x => x.Id == id);
            if (topicFromDb is null)
            {
                throw new TopicNotFoundException();
            }
            if (topicFromDb.Status == false)
            {
                throw new Exception("You can't update state of inactive topic");
            }
            if (topicFromDb.UserId != userId)
            {
                throw new Exception("You can't update other users state of topic");
            }
            var topicToPatch = _mapper.Map<StateUpdatingDto>(topicFromDb);
            patchDocument.ApplyTo(topicToPatch);
            _mapper.Map(topicToPatch, topicFromDb);

            await _topicRepository.Save();
        }
    }
}
