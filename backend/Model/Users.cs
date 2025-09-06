using System.ComponentModel.DataAnnotations;

namespace backend.Model
{


    public class User
    {
        public int Id { set; get; }

        public string Name { set; get; } = default!;

        public string Email { set; get; } = default!;

        public string Password { set; get; } = default!;

        public string Role { set; get; } = default!;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;


    }
}