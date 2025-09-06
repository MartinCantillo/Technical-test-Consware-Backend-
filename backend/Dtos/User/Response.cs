using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.User
{

    public class Response
    {

        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string Role { get; set; } = default!;

        public DateTime CreatedAtUtc { get; set; }

        public DateTime UpdatedAtUtc { get; set; }
    }
}