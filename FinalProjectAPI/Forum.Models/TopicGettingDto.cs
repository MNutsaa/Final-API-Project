using Forum.Entities;

namespace Forum.Models
{
    public class TopicGettingDto
    {
        public string Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime PublishDate { get; set; }
        public State State { get; set; }
        public bool Status { get; set; }
        public int CommentCount { get; set; }
        public ICollection<CommentsInTopicDto>? Comments { get; set; }
    }
}   
