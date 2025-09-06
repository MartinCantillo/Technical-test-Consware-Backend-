using backend.Model;
using backend.Utils;

namespace backend.Repository
{

    public interface ITravelRequest
    {
        Task<TravelRequest> CreateTravelRequest(TravelRequest travelRequest);

        Task<bool> UpdateStatus(int travelRequestId, TravelRequestStatus  newStatus);

        Task<IEnumerable<TravelRequest>> GetAll();

    }
}
