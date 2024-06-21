using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Entities
{
    public class Comment
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Content { get; set; } = null!;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; } = DateTime.Now;

        [Required]
        [ForeignKey(nameof(Topic))]
        public string TopicId { get; set; } = null!;

        public Topic Topic { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(Users))]
        public string UserId { get; set; } = null!;
        public Users User { get; set; } = null!;

    }
}
