using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.User
{

    public class UserLoginDto
    {
        [Required]
        public string Email { get; set; } = default!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;
    }
}