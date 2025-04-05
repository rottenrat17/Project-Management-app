# Project Management Application

This is a fork of the Project Management application by rottenrat17. The application helps manage projects and tasks with a modern web interface built using ASP.NET Core MVC.

## Features

- Project management with CRUD operations
- Task tracking within projects
- Project comments with AJAX functionality
- Search functionality for projects and tasks
- Asynchronous programming for better performance
- Area-based architecture for modular organization

## Technologies Used

- ASP.NET Core 9.0
- Entity Framework Core
- PostgreSQL Database
- jQuery and AJAX for dynamic content
- Bootstrap for UI

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL database

### Setup

1. Clone this repository
2. Update the connection string in `appsettings.json` to point to your PostgreSQL instance
3. Run the migrations to create the database: `dotnet ef database update`
4. Run the application: `dotnet run`

## Lab 9 Enhancements

This repository includes the following enhancements from Lab 9:

1. **Asynchronous Programming**:
   - Updated controller methods to use async/await pattern
   - Improved application responsiveness with asynchronous database operations

2. **AJAX Enhancements**:
   - Added ProjectComments feature with AJAX functionality
   - Dynamic comment loading and posting without page refresh
   - Real-time user interaction with the application

3. **Styling Improvements**:
   - Enhanced UI for project details page
   - Improved comment section styling with Bootstrap

## Future Enhancements

This repository will be updated with additional features and improvements as part of ongoing lab work. 