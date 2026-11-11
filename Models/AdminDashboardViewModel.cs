namespace TravelPlannerMvc.Models;

public class AdminDashboardViewModel
{
    public IReadOnlyList<UserAccount> Users { get; set; } = new List<UserAccount>();
    public IReadOnlyList<Destination> Destinations { get; set; } = new List<Destination>();
}
