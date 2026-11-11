using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using TravelPlannerMvc.Models;
using TravelPlannerMvc.Services;

namespace TravelPlannerMvc.Controllers;

public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly ITravelPlannerService _travelPlannerService;

    public AdminController(IUserService userService, ITravelPlannerService travelPlannerService)
    {
        _userService = userService;
        _travelPlannerService = travelPlannerService;
    }

    [HttpGet]
    public IActionResult Dashboard()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new AdminDashboardViewModel
        {
            Users = _userService.GetUsers(),
            Destinations = _travelPlannerService.GetDestinations()
        };

        return View(model);
    }

    [HttpGet]
    public IActionResult AddDestination()
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Account");
        }

        return View(new AddDestinationViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddDestination(AddDestinationViewModel model)
    {
        if (!IsAdmin())
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        _travelPlannerService.AddDestination(model);
        TempData["SuccessMessage"] = "Destination added successfully.";
        return RedirectToAction(nameof(Dashboard));
    }

    private bool IsAdmin()
    {
        return HttpContext.Session.GetString("UserRole") == "Admin";
    }
}
