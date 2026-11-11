using TravelPlannerMvc.Models;

namespace TravelPlannerMvc.Services;

public class TravelPlannerService : ITravelPlannerService
{
    private readonly object _destinationLock = new();

    private readonly List<Destination> _destinations = new()
    {
        new Destination
        {
            Id = 1,
            Name = "Hunza Valley",
            Region = "Northern Pakistan",
            Description = "A beautiful mountain valley known for scenery, culture, forts, rivers, and peaceful views.",
            EstimatedDailyBudget = 12000,
            BestFor = "Adventure, Nature, Family Trips",
            ImageUrl = "https://images.unsplash.com/photo-1589553416260-f586c8f1514f?auto=format&fit=crop&w=1200&q=80",
            Highlights = new List<string> { "Altit Fort", "Baltit Fort", "Attabad Lake", "Passu Cones", "Karimabad Market" }
        },
        new Destination
        {
            Id = 2,
            Name = "Skardu",
            Region = "Gilgit-Baltistan",
            Description = "A peaceful destination with lakes, deserts, mountains, and scenic valleys.",
            EstimatedDailyBudget = 14000,
            BestFor = "Adventure, Photography, Hiking",
            ImageUrl = "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1200&q=80",
            Highlights = new List<string> { "Shangrila Lake", "Upper Kachura Lake", "Cold Desert", "Shigar Fort", "Deosai Plains" }
        },
        new Destination
        {
            Id = 3,
            Name = "Murree",
            Region = "Punjab",
            Description = "A popular hill station suitable for short family trips and weekend travel.",
            EstimatedDailyBudget = 9000,
            BestFor = "Family, Short Trips, Budget Travel",
            ImageUrl = "https://images.unsplash.com/photo-1519681393784-d120267933ba?auto=format&fit=crop&w=1200&q=80",
            Highlights = new List<string> { "Mall Road", "Kashmir Point", "Pindi Point", "Patriata", "Bhurban" }
        },
        new Destination
        {
            Id = 4,
            Name = "Swat Valley",
            Region = "Khyber Pakhtunkhwa",
            Description = "A green valley with rivers, mountains, waterfalls, and family-friendly attractions.",
            EstimatedDailyBudget = 10000,
            BestFor = "Nature, Family, Relaxation",
            ImageUrl = "https://images.unsplash.com/photo-1501785888041-af3ef285b470?auto=format&fit=crop&w=1200&q=80",
            Highlights = new List<string> { "Malam Jabba", "Kalam", "Mahodand Lake", "Mingora", "Bahrain Bazaar" }
        }
    };

    public IReadOnlyList<Destination> GetDestinations()
    {
        lock (_destinationLock)
        {
            return _destinations.Select(CloneDestination).ToList();
        }
    }

    public Destination? GetDestinationById(int id)
    {
        lock (_destinationLock)
        {
            var destination = _destinations.FirstOrDefault(d => d.Id == id);
            return destination == null ? null : CloneDestination(destination);
        }
    }

    public Destination? FindDestination(string name)
    {
        lock (_destinationLock)
        {
            var destination = _destinations.FirstOrDefault(d =>
                d.Name.Equals(name, StringComparison.OrdinalIgnoreCase) ||
                d.Name.Contains(name, StringComparison.OrdinalIgnoreCase));

            return destination == null ? null : CloneDestination(destination);
        }
    }

    public Destination AddDestination(AddDestinationViewModel model)
    {
        lock (_destinationLock)
        {
            var nextId = _destinations.Count == 0 ? 1 : _destinations.Max(d => d.Id) + 1;
            var destination = new Destination
            {
                Id = nextId,
                Name = model.Name.Trim(),
                Region = model.Region.Trim(),
                Description = model.Description.Trim(),
                EstimatedDailyBudget = model.EstimatedDailyBudget,
                BestFor = model.BestFor.Trim(),
                ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl)
                    ? "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=1200&q=80"
                    : model.ImageUrl.Trim(),
                Highlights = ParseHighlights(model.HighlightsText)
            };

            _destinations.Add(destination);
            return CloneDestination(destination);
        }
    }

    private static List<string> ParseHighlights(string highlightsText)
    {
        return highlightsText
            .Split(new[] { ',', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(h => h.Trim())
            .Where(h => !string.IsNullOrWhiteSpace(h))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    private static Destination CloneDestination(Destination destination)
    {
        return new Destination
        {
            Id = destination.Id,
            Name = destination.Name,
            Region = destination.Region,
            Description = destination.Description,
            EstimatedDailyBudget = destination.EstimatedDailyBudget,
            BestFor = destination.BestFor,
            ImageUrl = destination.ImageUrl,
            Highlights = destination.Highlights.ToList()
        };
    }

    public TripPlan GeneratePlan(TripRequest request)
    {
        var destination = FindDestination(request.Destination) ?? GetDestinations().First();

        var plan = new TripPlan
        {
            Request = request,
            Destination = destination
        };

        var cumulativeCost = 0;
        for (int day = 1; day <= request.Duration; day++)
        {
            var dayPlan = CreateDayPlan(day, request.Duration, destination, request.TravelType, request.Travelers);
            cumulativeCost += dayPlan.TotalCost;
            dayPlan.CumulativeCost = cumulativeCost;
            dayPlan.RemainingBudgetAfterDay = request.Budget - cumulativeCost;
            plan.Days.Add(dayPlan);
        }

        plan.EstimatedTotalCost = plan.Days.Sum(d => d.TotalCost);
        plan.RemainingBudget = request.Budget - plan.EstimatedTotalCost;
        plan.BudgetStatus = BuildBudgetStatus(request.Budget, plan.EstimatedTotalCost, plan.RemainingBudget);
        plan.Tips = BuildTips(request, destination, plan.RemainingBudget);

        return plan;
    }

    private static DayPlan CreateDayPlan(int day, int duration, Destination destination, string travelType, int travelers)
    {
        var highlights = destination.Highlights.Count > 0 ? destination.Highlights : new List<string> { destination.Name };
        var mainPlace = highlights[(day - 1) % highlights.Count];
        var secondPlace = highlights[day % highlights.Count];
        var dailyCost = CalculateDailyCost(destination.EstimatedDailyBudget, travelers, travelType, day, duration);
        var costBreakdown = BuildCostBreakdown(dailyCost);

        var dayPlan = travelType.ToLower() switch
        {
            "family" => new DayPlan
            {
                DayNumber = day,
                Title = $"Relaxed family day around {mainPlace}",
                Morning = $"Start with breakfast, then visit {mainPlace} at a comfortable pace.",
                Afternoon = $"Have lunch nearby and spend time around {secondPlace}.",
                Evening = "Return to hotel early, have dinner, and prepare for the next day."
            },
            "budget" => new DayPlan
            {
                DayNumber = day,
                Title = $"Budget-friendly visit to {mainPlace}",
                Morning = $"Use local transport or shared car options to reach {mainPlace}.",
                Afternoon = $"Eat at a local restaurant and visit {secondPlace} without expensive add-ons.",
                Evening = "Walk around the local market and keep shopping controlled."
            },
            "luxury" => new DayPlan
            {
                DayNumber = day,
                Title = $"Comfort trip around {mainPlace}",
                Morning = $"Book private transport and visit {mainPlace} with a relaxed schedule.",
                Afternoon = $"Enjoy lunch at a good restaurant and continue to {secondPlace}.",
                Evening = "Dinner at a premium hotel or scenic restaurant."
            },
            _ => new DayPlan
            {
                DayNumber = day,
                Title = $"Adventure day at {mainPlace}",
                Morning = $"Leave early and explore {mainPlace} while the weather is good.",
                Afternoon = $"Continue towards {secondPlace}, take photos, and enjoy local food.",
                Evening = "Return safely before late night and review tomorrow's route."
            }
        };

        dayPlan.TransportCost = costBreakdown.Transport;
        dayPlan.AccommodationCost = costBreakdown.Accommodation;
        dayPlan.FoodCost = costBreakdown.Food;
        dayPlan.ActivityCost = costBreakdown.Activity;

        return dayPlan;
    }

    private static int CalculateDailyCost(int estimatedDailyBudget, int travelers, string travelType, int day, int duration)
    {
        var baseCost = estimatedDailyBudget * travelers;

        decimal travelTypeMultiplier = travelType.ToLower() switch
        {
            "budget" => 0.85m,
            "family" => 1.00m,
            "luxury" => 1.35m,
            "adventure" => 1.10m,
            _ => 1.00m
        };

        decimal dayMultiplier = day switch
        {
            1 => 1.10m,
            _ when day == duration => 1.05m,
            _ => 1.00m
        };

        return RoundToNearestHundred(baseCost * travelTypeMultiplier * dayMultiplier);
    }

    private static (int Transport, int Accommodation, int Food, int Activity) BuildCostBreakdown(int totalCost)
    {
        var transport = RoundToNearestHundred(totalCost * 0.25m);
        var accommodation = RoundToNearestHundred(totalCost * 0.35m);
        var food = RoundToNearestHundred(totalCost * 0.22m);
        var activity = totalCost - transport - accommodation - food;

        return (transport, accommodation, food, activity);
    }

    private static int RoundToNearestHundred(decimal value)
    {
        return (int)(Math.Round(value / 100m, MidpointRounding.AwayFromZero) * 100m);
    }

    private static string BuildBudgetStatus(int totalBudget, int estimatedCost, int remainingBudget)
    {
        if (remainingBudget < 0)
        {
            return "Your budget may be low. The plan goes over your budget, so consider reducing hotel cost, transport cost, or trip duration.";
        }

        if (totalBudget > 0 && remainingBudget > totalBudget * 0.25m)
        {
            return "You are not using all of your budget. You can keep the remaining money as saving/emergency cash, or upgrade hotels, transport, and activities.";
        }

        return "Your budget looks well balanced for this trip.";
    }

    private static List<string> BuildTips(TripRequest request, Destination destination, int remainingBudget)
    {
        var tips = new List<string>
        {
            "Keep a small emergency budget separate from your main travel budget.",
            "Confirm hotel booking and transport before departure.",
            "Check road and weather conditions before travelling to mountain areas."
        };

        if (remainingBudget < 0)
        {
            tips.Add("Choose budget hotels or reduce the trip by one day to control cost.");
        }
        else if (remainingBudget > request.Budget * 0.25m)
        {
            tips.Add("Your remaining budget is high, so you can either save it or improve comfort by upgrading hotel, transport, or activities.");
        }

        if (destination.Region.Contains("Northern", StringComparison.OrdinalIgnoreCase) || destination.Region.Contains("Gilgit", StringComparison.OrdinalIgnoreCase))
        {
            tips.Add("Carry warm clothes, power bank, cash, and basic medicine for northern areas.");
        }

        if (!string.IsNullOrWhiteSpace(request.Notes))
        {
            tips.Add($"Special note considered: {request.Notes}");
        }

        return tips;
    }
}
