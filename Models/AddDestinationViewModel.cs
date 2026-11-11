using System.ComponentModel.DataAnnotations;

namespace TravelPlannerMvc.Models;

public class AddDestinationViewModel
{
    [Required(ErrorMessage = "Please enter the destination name.")]
    [Display(Name = "Destination Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the region.")]
    public string Region { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a short description.")]
    [MinLength(20, ErrorMessage = "Description should be at least 20 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter the estimated daily budget.")]
    [Range(1000, 1000000, ErrorMessage = "Daily budget should be between 1,000 and 1,000,000 PKR.")]
    [Display(Name = "Estimated Daily Budget")]
    public int EstimatedDailyBudget { get; set; }

    [Required(ErrorMessage = "Please enter what this destination is best for.")]
    [Display(Name = "Best For")]
    public string BestFor { get; set; } = string.Empty;

    [Display(Name = "Image URL")]
    public string? ImageUrl { get; set; }

    [Required(ErrorMessage = "Please enter at least one highlight.")]
    [Display(Name = "Highlights")]
    public string HighlightsText { get; set; } = string.Empty;
}
