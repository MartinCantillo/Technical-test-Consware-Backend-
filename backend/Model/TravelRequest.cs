using System.ComponentModel.DataAnnotations;
using backend.Utils;

namespace backend.Model
{
    public class TravelRequest : IValidatableObject
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required(ErrorMessage = "La ciudad de origen es obligatoria.")]
        public string OriginCity { get; set; } = default!;

        [Required(ErrorMessage = "La ciudad de destino es obligatoria.")]
        public string DestinationCity { get; set; } = default!;

        [DataType(DataType.Date)]
        public DateOnly DepartureDate { get; set; }

        [DataType(DataType.Date)]
        public DateOnly ReturnDate { get; set; }

        [Required(ErrorMessage = "La justificación es obligatoria.")]
        [MaxLength(1000, ErrorMessage = "La justificación no puede superar los 1000 caracteres.")]
        public string Justification { get; set; } = default!;

        public TravelRequestStatus Status { get; set; } = TravelRequestStatus.pending;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

        // 🔎 Validaciones personalizadas
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DepartureDate > ReturnDate)
            {
                yield return new ValidationResult(
                    "La fecha de regreso debe ser mayor a la fecha de ida.",
                    new[] { nameof(ReturnDate), nameof(DepartureDate) }
                );
            }

            if (OriginCity == DestinationCity)
            {
                yield return new ValidationResult(
                    "La ciudad de origen y destino no pueden ser iguales.",
                    new[] { nameof(OriginCity), nameof(DestinationCity) }
                );
            }
        }
    }
}
