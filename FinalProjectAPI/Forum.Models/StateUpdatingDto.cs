using Forum.Entities;
using System.ComponentModel.DataAnnotations;

namespace Forum.Models
{
    public class StateUpdatingDto
    {
        [Required]
        public State State { get; set; }
    }
}
