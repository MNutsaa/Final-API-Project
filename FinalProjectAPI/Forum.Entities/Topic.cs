using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Forum.Entities
{
    public class Topic
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Title { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        [DataType(DataType.Date)]
        public DateTime PublishDate { get; set; } = DateTime.Now;

        [Required]
        public State State { get; set; } = State.Pending;

        [Required]
        public bool Status { get; set; } = true;

        [Required]
        [ForeignKey(nameof(Users))]
        public string UserId { get; set; } = null!;
        public Users User { get; set; } = null!;
        public ICollection<Comment>? Comments { get; set; }
    }
}
