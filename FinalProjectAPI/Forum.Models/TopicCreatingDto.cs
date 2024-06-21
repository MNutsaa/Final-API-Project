using Forum.Entities;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class TopicCreatingDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public State State { get; set; } = State.Pending;

        [Required]
        public bool Status { get; set; } = true;
    }
}
