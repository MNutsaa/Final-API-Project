using Microsoft.AspNetCore.Identity;

namespace Forum.Entities
{
    public class Users : IdentityUser
    {
        public string Name { get; set; } = null!;
        public ICollection<Topic>? Topic { get; set; }
        public ICollection<Comment>? Comment { get; set; }
    }
}
