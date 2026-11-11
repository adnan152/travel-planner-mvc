using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMvc.Models;

public class TripRequest
{
    [Required(ErrorMessage = "Please choose a destination.")]
    [Display(Name = "Destination")]
    public string Destination { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter your total budget.")]
    [Range(5000, 10000000, ErrorMessage = "Budget must be at least 5,000.")]
    [Display(Name = "Total Budget")]
    public int Budget { get; set; } = 50000;

    [Required]
    [Range(1, 30, ErrorMessage = "Duration must be between 1 and 30 days.")]
    [Display(Name = "Duration in Days")]
    public int Duration { get; set; } = 3;

    [Required]
    [Display(Name = "Travel Type")]
    public string TravelType { get; set; } = "Adventure";

    [Display(Name = "Number of Travelers")]
    [Range(1, 20, ErrorMessage = "Travelers must be between 1 and 20.")]
    public int Travelers { get; set; } = 1;

    [Display(Name = "Special Notes")]
    public string? Notes { get; set; }
}
