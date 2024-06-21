namespace Forum.Models
{
    public class CommentsInTopicDto
    {
        public string Name { get; set; }
        public string Context { get; set; } = null!;
        public DateTime CreatedTime { get; set; }
    }
}
