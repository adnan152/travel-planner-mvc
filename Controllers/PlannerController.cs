using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TravelPlannerMvc.Models;
using TravelPlannerMvc.Services;

namespace TravelPlannerMvc.Controllers;

public class PlannerController : Controller
{
    private readonly ITravelPlannerService _travelPlannerService;

    public PlannerController(ITravelPlannerService travelPlannerService)
    {
        _travelPlannerService = travelPlannerService;
    }

    [HttpGet]
    public IActionResult Index(string? destination)
    {
        if (!IsLoggedIn())
        {
            TempData["LoginMessage"] = "Please login first to plan your trip.";
            var returnUrl = Url.Action("Index", "Planner", new { destination });
            return RedirectToAction("Login", "Account", new { returnUrl });
        }

        ViewBag.Destinations = _travelPlannerService.GetDestinations();

        var request = new TripRequest
        {
            Destination = destination ?? "Hunza Valley",
            Budget = 50000,
            Duration = 3,
            Travelers = 1,
            TravelType = "Adventure"
        };

        return View(request);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Generate(TripRequest request)
    {
        if (!IsLoggedIn())
        {
            TempData["LoginMessage"] = "Please login first to generate your travel plan.";
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Index", "Planner") });
        }

        ViewBag.Destinations = _travelPlannerService.GetDestinations();

        if (!ModelState.IsValid)
        {
            return View("Index", request);
        }

        var plan = _travelPlannerService.GeneratePlan(request);
        return View("Result", plan);
    }

    private bool IsLoggedIn()
    {
        return !string.IsNullOrEmpty(HttpContext.Session.GetString("UserEmail"));
    }
}
