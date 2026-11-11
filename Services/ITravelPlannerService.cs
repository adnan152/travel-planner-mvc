using TravelPlannerMvc.Models;

namespace TravelPlannerMvc.Services;

public interface ITravelPlannerService
{
    IReadOnlyList<Destination> GetDestinations();
    Destination? GetDestinationById(int id);
    Destination? FindDestination(string name);
    Destination AddDestination(AddDestinationViewModel model);
    TripPlan GeneratePlan(TripRequest request);
}
