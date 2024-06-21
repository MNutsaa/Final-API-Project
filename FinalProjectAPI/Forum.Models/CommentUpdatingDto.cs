using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class CommentUpdatingDto
    {
        [Required]
        [MaxLength(500)]
        public string Content { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; } = DateTime.Now;
    }
}
