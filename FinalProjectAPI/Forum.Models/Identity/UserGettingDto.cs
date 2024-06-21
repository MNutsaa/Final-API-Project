namespace Forum.Models.Identity
{
    public class UserGettingDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public ICollection<TopicGettingDto>? Topics { get; set; }
        public ICollection<CommentGettingDto>? Comments { get; set; }
    }
}
