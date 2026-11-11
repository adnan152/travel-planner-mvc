namespace TravelPlannerMvc.Models;

public class DayPlan
{
    public int DayNumber { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Morning { get; set; } = string.Empty;
    public string Afternoon { get; set; } = string.Empty;
    public string Evening { get; set; } = string.Empty;

    public int TransportCost { get; set; }
    public int AccommodationCost { get; set; }
    public int FoodCost { get; set; }
    public int ActivityCost { get; set; }
    public int TotalCost => TransportCost + AccommodationCost + FoodCost + ActivityCost;

    public int CumulativeCost { get; set; }
    public int RemainingBudgetAfterDay { get; set; }
}
