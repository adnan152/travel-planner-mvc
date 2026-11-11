using TravelPlannerMvc.Models;

namespace TravelPlannerMvc.Services;

public interface IUserService
{
    IReadOnlyList<UserAccount> GetUsers();
    UserAccount? ValidateUser(string email, string password);
    bool EmailExists(string email);
    UserAccount Register(RegisterViewModel model);
}
