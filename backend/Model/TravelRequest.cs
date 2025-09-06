using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Model
{
    public class TravelRequest
    {
        public int Id { set; get; }

        public int UserId { set; get; }


        [Required]
        public string OriginCity { set; get; } = default!;


        [Required]
        public string DestinationCity { set; get; } = default!;


        [DataType(DataType.Date)]
        public DateOnly DepartureDate { set; get; }

        [DataType(DataType.Date)]
        public DateOnly ReturnDate { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Justification { get; set; } = default!;

        public TravelRequestStatus Status { get; set; } = TravelRequestStatus.pending;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;




    }
}