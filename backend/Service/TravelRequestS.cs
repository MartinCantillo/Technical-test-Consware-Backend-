using backend.Data;
using backend.Model;
using backend.Repository;
using backend.Utils;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace backend.Service
{
    public class TravelRequestS : ITravelRequest
    {
        private readonly DataContext _context;

        public TravelRequestS(DataContext context)
        {
            _context = context;
        }

        public async Task<TravelRequest> CreateTravelRequest(TravelRequest travelRequest)
        {
            try
            {
                if (travelRequest == null)
                    throw new ArgumentNullException(nameof(travelRequest));

                
                var validationContext = new ValidationContext(travelRequest);
                var validationResults = new List<ValidationResult>();
                bool isValid = Validator.TryValidateObject(travelRequest, validationContext, validationResults, true);

                if (!isValid)
                {
                    var errors = string.Join("; ", validationResults.Select(v => v.ErrorMessage));
                    throw new ValidationException($"Errores de validación: {errors}");
                }

                await _context.TravelRequests.AddAsync(travelRequest);
                await _context.SaveChangesAsync();

                return travelRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[CreateTravelRequest] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<TravelRequest>> GetAll()
        {
            try
            {
                return await _context.TravelRequests.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[GetAll] Error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> UpdateStatus(int travelRequestId, TravelRequestStatus  newStatus)
        {
            try
            {
                var request = await _context.TravelRequests.FirstOrDefaultAsync(t => t.Id == travelRequestId);
                if (request == null) return false;

                request.Status = newStatus;
                request.UpdatedAtUtc = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UpdateStatus] Error: {ex.Message}");
                throw;
            }
        }
    }
}
