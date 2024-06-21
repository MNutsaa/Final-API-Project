using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class CommentCreatingDto
    {
        [Required]
        [MaxLength(5000)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

    }
}
