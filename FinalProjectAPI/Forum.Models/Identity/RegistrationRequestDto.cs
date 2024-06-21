using System.ComponentModel.DataAnnotations;

namespace Forum.Models.Identity
{
    public class RegistrationRequestDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
