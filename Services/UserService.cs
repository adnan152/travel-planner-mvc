using TravelPlannerMvc.Models;

namespace TravelPlannerMvc.Services;

public class UserService : IUserService
{
    private readonly object _lock = new();
    private readonly List<UserAccount> _users = new()
    {
        new UserAccount
        {
            Id = 1,
            FullName = "System Admin",
            Email = "admin@travelplanner.com",
            Password = "Admin@123",
            Role = "Admin",
            CreatedAt = DateTime.Now
        },
        new UserAccount
        {
            Id = 2,
            FullName = "Demo Customer",
            Email = "customer@example.com",
            Password = "Customer@123",
            Role = "Customer",
            CreatedAt = DateTime.Now
        }
    };

    public IReadOnlyList<UserAccount> GetUsers()
    {
        lock (_lock)
        {
            return _users.Select(u => new UserAccount
            {
                Id = u.Id,
                FullName = u.FullName,
                Email = u.Email,
                Password = u.Password,
                Role = u.Role,
                CreatedAt = u.CreatedAt
            }).ToList();
        }
    }

    public UserAccount? ValidateUser(string email, string password)
    {
        lock (_lock)
        {
            return _users.FirstOrDefault(u =>
                u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase) &&
                u.Password == password);
        }
    }

    public bool EmailExists(string email)
    {
        lock (_lock)
        {
            return _users.Any(u => u.Email.Equals(email.Trim(), StringComparison.OrdinalIgnoreCase));
        }
    }

    public UserAccount Register(RegisterViewModel model)
    {
        lock (_lock)
        {
            var nextId = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
            var user = new UserAccount
            {
                Id = nextId,
                FullName = model.FullName.Trim(),
                Email = model.Email.Trim(),
                Password = model.Password,
                Role = "Customer",
                CreatedAt = DateTime.Now
            };

            _users.Add(user);
            return user;
        }
    }
}
