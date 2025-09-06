using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.User
{

    public class UserCreateDto
    {
        [Required]
        public string Name { get; set; } = default!;


        [Required]
        public string Email { get; set; } = default!;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = default!;

        [Required]
        public string Role { get; set; } = default!;
    }
}