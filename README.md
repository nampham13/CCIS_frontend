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

## Setup & Run

Prerequisites:

- **.NET SDK:** Install .NET 10 SDK (recommended) or the matching runtime. Verify with:

```powershell
dotnet --version
dotnet --list-runtimes
```

Quick start (project directory):

```powershell
dotnet restore
dotnet build
dotnet run
# or for live reload while editing:
dotnet watch run
```

Local URLs (from `Properties/launchSettings.json`):

- http://localhost:5080
- https://localhost:7080

If you need HTTPS without browser warnings (developer cert):

```powershell
dotnet dev-certs https --trust
```

IDE options:

- **Visual Studio:** Open the solution `CCIS.sln` and press F5 (or Ctrl+F5 to run without debugger).
- **VS Code:** Open the folder, install the C# extension, then use the Run view or the integrated terminal to run the commands above.

Troubleshooting:

- If the app reports a missing runtime (e.g., requires `Microsoft.NETCore.App` 8.0.0), install the matching runtime or retarget the project in `CCIS.csproj` by changing `<TargetFramework>`.
- If Razor compilation errors reference tag helpers in attributes (RZ1031), avoid inline C# in element attributes; use conditional rendering in views instead.

## Notes

- The current implementation uses in-memory mock data, so user edits and acknowledgements are not persisted yet.
- Launch settings are configured in `Properties/launchSettings.json` for local development.

## Next Steps

1. Replace the mock service with a database-backed repository.
2. Add POST actions and validation for user management and alert acknowledgements.
"# CCIS_frontend" 
