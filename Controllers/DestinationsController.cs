using Microsoft.AspNetCore.Mvc;
using TravelPlannerMvc.Services;

namespace TravelPlannerMvc.Controllers;

public class DestinationsController : Controller
{
    private readonly ITravelPlannerService _travelPlannerService;

    public DestinationsController(ITravelPlannerService travelPlannerService)
    {
        _travelPlannerService = travelPlannerService;
    }

    public IActionResult Index()
    {
        return View(_travelPlannerService.GetDestinations());
    }

    public IActionResult Details(int id)
    {
        var destination = _travelPlannerService.GetDestinationById(id);
        if (destination == null)
        {
            return NotFound();
        }

        return View(destination);
    }
}
