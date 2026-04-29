# CCIS

CCIS is now an ASP.NET Core MVC application for the Smart Access Control Dashboard. The old React/Vite prototype has been removed, and the repository now contains only the server-rendered .NET application and supporting assets.

## What Changed

- Migrated the app runtime to ASP.NET Core MVC.
- Added server-rendered pages for Dashboard, Face Access, User Management, Access Logs, and Alerts Center.
- Moved the mock data and page models into C# services and view models.
- Rebuilt the UI with a custom dark industrial theme in `wwwroot/css/site.css`.

## Project Structure

```
CCIS/
├── CCIS.csproj
├── CCIS.sln
├── Program.cs
├── Controllers/
├── Models/
├── Services/
├── Views/
├── Properties/
├── wwwroot/
└── appsettings.json
```

## Run It

1. Install the .NET 8 SDK.
2. Open the `CCIS.csproj` project in Visual Studio or VS Code.
3. Run the app with `dotnet run` from the project directory.

## Notes

- The current implementation uses in-memory mock data, so user edits and acknowledgements are not persisted yet.
- Launch settings are configured in `Properties/launchSettings.json` for local development.

## Next Steps

1. Replace the mock service with a database-backed repository.
2. Add POST actions and validation for user management and alert acknowledgements.
"# CCIS_frontend" 
