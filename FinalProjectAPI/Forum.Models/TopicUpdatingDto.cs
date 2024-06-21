using Forum.Entities;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class TopicUpdatingDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public State State { get; set; }
    }
}
