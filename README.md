# Travel Planner MVC

This is a beginner-friendly ASP.NET Core MVC project in C#.

## Features

- Home page
- Destination listing
- Destination details page
- Trip planner form
- Form validation
- Auto-generated day-by-day itinerary
- Budget calculation
- Daily budget breakdown for day 1, day 2, day 3, etc.
- Remaining budget after each day
- One shared login page for both Customer and Admin
- Customer signup only
- Admin login using the pre-created admin account
- Customer dashboard
- Admin dashboard with registered users and destinations
- Admin can add new destinations
- Newly added destinations appear in the destination listing and trip planner
- Simple responsive CSS styling
- Clean MVC structure with a service layer

## Demo Login Details

### Admin

```text
Email: admin@travelplanner.com
Password: Admin@123
```

### Customer

```text
Email: customer@example.com
Password: Customer@123
```

The same login page is used for both customer and admin users. The system checks the user's role and redirects them to the correct dashboard.

## Admin Destination Management

After logging in as admin:

1. Open **Admin Dashboard**.
2. Click **Add New Destination**.
3. Enter the destination name, region, daily budget, best-for text, image URL, description, and highlights.
4. Submit the form.
5. The destination will appear in the admin destination table, public destination listing, and trip planner.

## Project Structure

```text
TravelPlannerMvc/
├── Controllers/
│   ├── AccountController.cs
│   ├── AdminController.cs
│   ├── HomeController.cs
│   ├── DestinationsController.cs
│   └── PlannerController.cs
├── Models/
│   ├── AddDestinationViewModel.cs
│   ├── AdminDashboardViewModel.cs
│   ├── Destination.cs
│   ├── DayPlan.cs
│   ├── LoginViewModel.cs
│   ├── RegisterViewModel.cs
│   ├── TripRequest.cs
│   ├── TripPlan.cs
│   └── UserAccount.cs
├── Services/
│   ├── ITravelPlannerService.cs
│   ├── TravelPlannerService.cs
│   ├── IUserService.cs
│   └── UserService.cs
├── Views/
│   ├── Account/
│   ├── Admin/
│   ├── Home/
│   ├── Destinations/
│   ├── Planner/
│   └── Shared/
├── wwwroot/css/site.css
├── Program.cs
└── TravelPlannerMvc.csproj
```

## How to Run

1. Install the .NET SDK.
2. Open a terminal inside this project folder.
3. Run:

```bash
dotnet restore
dotnet run
```

4. Open the URL shown in the terminal, usually:

```text
https://localhost:5001
```

or

```text
http://localhost:5000
```

## How to Run in Visual Studio

1. Open Visual Studio.
2. Choose **Open a project or solution**.
3. Select `TravelPlannerMvc.csproj`.
4. Press **Ctrl + F5** or click the green Run button.

## Suggested Presentation Explanation

This project follows the MVC architecture:

- **Model**: stores data classes such as Destination, TripRequest, TripPlan, DayPlan, UserAccount, LoginViewModel, RegisterViewModel, and AddDestinationViewModel.
- **View**: displays HTML pages using Razor syntax.
- **Controller**: handles user requests and returns views.
- **Service**: contains business logic for generating travel plans, managing demo users, and managing destinations.

## Important Note

This version uses in-memory demo users and destinations so it is easy to run and explain. When the app restarts, newly registered users and newly added destinations are reset. For a real project, connect it to SQL Server using Entity Framework Core and store hashed passwords instead of plain text passwords.

## Future Improvements

- Add SQL Server database using Entity Framework Core
- Save generated trips for each customer
- Add edit/delete destination options for admin
- Add hotel and transport booking modules
- Add AI-based destination recommendation

## Login Required for Trip Planning

The Plan Trip page now checks whether the visitor is logged in. If a visitor clicks **Plan Trip** without logging in, the site redirects them to the shared login page and shows a message asking them to login first. After successful login, the visitor is sent back to the Plan Trip page.
