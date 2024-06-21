using Forum.Models;
using Microsoft.AspNetCore.JsonPatch;

namespace Forum.Contracts
{
    public interface ICommentService
    {
        Task<List<CommentGettingDto>> GetCommentsOfUserAsync(string userId);
        Task<CommentGettingDto> GetSingleCommentByUserId(string commentId, string userId);
        Task DeleteCommentAsync(string commentid);
        Task AddCommentAsync(string topicId, CommentCreatingDto commentCreatingDto);
        Task UpdateCommentAsync(string comementId, JsonPatchDocument<CommentUpdatingDto> patchDocument);
    }
}
