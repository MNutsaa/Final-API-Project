using AutoMapper;
using Forum.Contracts;
using Forum.Entities;
using Forum.Models;
using Forum.Service.Exceptions;
using Microsoft.AspNetCore.JsonPatch;

namespace Forum.Service.Implementations
{
    public class CommentService : ICommentService
    {

        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;
        private readonly ITopicReporitory _topicRepository;
        private readonly IUserRepository _userRepository;

        public CommentService(ICommentRepository commentRepository, ITopicReporitory topicRepository, IUserRepository userRepository, MappingInitializer mappingInitializer)
        {
            _commentRepository = commentRepository;
            _topicRepository = topicRepository;
            _userRepository = userRepository;
            _mapper = mappingInitializer.Initialize();
        }
        public async Task AddCommentAsync(string topicId, CommentCreatingDto commentCreatingDto)
        {
            if (topicId == string.Empty)
            {
                throw new TopicNotFoundException();
            }
            if (commentCreatingDto == null)
            {
                throw new ArgumentNullException("Argument doen't exist");
            }

            var topic = await _topicRepository.GetSingleTopicAsync(x => x.Id == topicId);

            if (topic == null)
            {
                throw new TopicNotFoundException();
            }
            if (topic.Status != true)
            {
                throw new Exception("Topic is inactive");
            }

            var result = _mapper.Map<Comment>(commentCreatingDto);
            var userId = _userRepository.GetAuthenticatedUserId();
            if (userId is null)
            {
                throw new UnauthorizedAccessException("Must be logged in to create Comment");
            }
            result.UserId = userId;
            result.TopicId = topicId;

            await _commentRepository.AddCommentAsync(result);
            await _commentRepository.Save();
        }

        public async Task DeleteCommentAsync(string id)
        {
            if (id == string.Empty)
                throw new ArgumentException("Invalid argument passed");

            var rawComment = await _commentRepository.GetSingleCommentAsync(x => x.Id == id);

            if (rawComment == null)
                throw new CommentNotFoundException();

            var userId = _userRepository.GetAuthenticatedUserId();
            if (userId is null)
            {
                throw new UnauthorizedAccessException("Please log in");
            }
            if (rawComment.UserId.Trim() != userId && _userRepository.GetAuthenticatedUserRole().Trim() != "Admin")
            {
                throw new Exception("You can't delete comments which doesn't belong to you");
            }
            else
            {
                _commentRepository.DeleteComment(rawComment);
                await _commentRepository.Save();
            }
        }

        public async Task<CommentGettingDto> GetSingleCommentByUserId(string commentId, string userId)
        {

            if (commentId == string.Empty || string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Invalid argument passed");

            var rawComment = await _commentRepository.GetSingleCommentAsync(x => x.Id == commentId && x.UserId == userId, includePropeties: "Topic,User");

            if (rawComment == null)
                throw new TopicNotFoundException();

            var result = _mapper.Map<CommentGettingDto>(rawComment);
            return result;
        }

        public async Task<List<CommentGettingDto>> GetCommentsOfUserAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("Invalid argument passed");

            var rawComments = await _commentRepository.GetAllCommentsAsync(x => x.UserId.Trim() == userId.Trim(), includePropeties: "Topic,User");
            List<CommentGettingDto> result = new();

            if (rawComments.Count > 0)

                result = _mapper.Map<List<CommentGettingDto>>(rawComments);

            return result;
        }
        public async Task UpdateCommentAsync(string commentId, JsonPatchDocument<CommentUpdatingDto> patchDocument)
        {

            if (patchDocument is null)
            {
                throw new ArgumentNullException("Invalid argument passed");
            }
            var userId = _userRepository.GetAuthenticatedUserId();

            if (userId is null)
            {
                throw new UnauthorizedAccessException("Please log in to update comment");
            }
            var comment = await _commentRepository.GetSingleCommentAsync(x => x.Id == commentId);

            if (comment is null)
            {
                throw new CommentNotFoundException();
            }
            if (comment.UserId != userId)
            {
                throw new Exception("You can't delete comments which doesn't belong to you");
            }
            var commentToPatch = _mapper.Map<CommentUpdatingDto>(comment);
            patchDocument.ApplyTo(commentToPatch);
            _mapper.Map(commentToPatch, comment);

            await _commentRepository.Save();
        }
    }
}
