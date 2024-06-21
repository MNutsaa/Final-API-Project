namespace Forum.Models
{
    public class CommentGettingDto
    {
        public string Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
        public string TopicTitle { get; set; } = null!;
        public string UserName { get; set; }
    }
}
