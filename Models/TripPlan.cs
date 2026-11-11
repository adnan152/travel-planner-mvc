namespace TravelPlannerMvc.Models;

public class TripPlan
{
    public TripRequest Request { get; set; } = new();
    public Destination? Destination { get; set; }
    public int EstimatedTotalCost { get; set; }
    public int RemainingBudget { get; set; }
    public string BudgetStatus { get; set; } = string.Empty;
    public List<DayPlan> Days { get; set; } = new();
    public List<string> Tips { get; set; } = new();
}
