using System.ComponentModel.DataAnnotations;

namespace backend.Dtos.TravelRequest
{

    public class TravelRequestCreateDto
    {
        [Required]
        public int UserId { get; set; }

        [Required]
        public string OriginCity { get; set; } = default!;

        [Required]
        public string DestinationCity { get; set; } = default!;

        [Required]
        [DataType(DataType.Date)]
        public DateOnly DepartureDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly ReturnDate { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Justification { get; set; } = default!;
    }
}