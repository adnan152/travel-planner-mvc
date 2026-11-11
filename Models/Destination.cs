namespace TravelPlannerMvc.Models;

public class Destination
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int EstimatedDailyBudget { get; set; }
    public string BestFor { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public List<string> Highlights { get; set; } = new();
}
