using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Dtos.TravelRequest
{

    public class TravelRequestUpdateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public TravelRequestStatus Status { get; set; }
    }
}