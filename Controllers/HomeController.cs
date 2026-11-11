using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TravelPlannerMvc.Models;
using TravelPlannerMvc.Services;

namespace TravelPlannerMvc.Controllers;

public class HomeController : Controller
{
    private readonly ITravelPlannerService _travelPlannerService;

    public HomeController(ITravelPlannerService travelPlannerService)
    {
        _travelPlannerService = travelPlannerService;
    }

    public IActionResult Index()
    {
        var destinations = _travelPlannerService.GetDestinations().Take(3).ToList();
        return View(destinations);
    }

    public IActionResult About()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
