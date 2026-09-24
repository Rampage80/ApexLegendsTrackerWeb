# ApexLegendsTracker Web

Blazor WebAssembly frontend for ApexLegendsTracker.

## Prerequisites

- .NET 10 SDK

## Configuration

Set the backend API base URL in:

- `ApexLegendsTracker.Web/wwwroot/appsettings.json` (local development default)
- `ApexLegendsTracker.Web/wwwroot/appsettings.Production.json` (deployed/Azure override)

Property:

- `ApiBaseUrl` (local default: `http://localhost:5165/`; production: the deployed backend App Service URL)

Blazor WebAssembly automatically loads `appsettings.{Environment}.json` on top of `appsettings.json` based on the app's hosting environment (`Development` locally via `dotnet run`/`ASPNETCORE_ENVIRONMENT`, `Production` when served as static files from Azure App Service). No environment variables or code changes are needed to switch the API URL between environments — just keep both JSON files in sync with the correct backend host.

## Run

```powershell
dotnet run --project ./src/ApexLegendsTracker.Web
```

## Test

```powershell
dotnet test
```

## Browser accessibility tests

Start the Blazor app in another terminal, then install the Playwright browser once:

```powershell
dotnet run --project ./ApexLegendsTracker.Web --urls http://localhost:5001
dotnet build ./ApexLegendsTracker.Web.Tests
pwsh ./ApexLegendsTracker.Web.Tests/bin/Debug/net10.0/playwright.ps1 install chromium
```

Run the UI and WCAG-focused checks against that app:

```powershell
$env:PLAYWRIGHT_BASE_URL = "http://localhost:5001"
dotnet test ./ApexLegendsTracker.Web.Tests --filter FullyQualifiedName~HomeAccessibilityPlaywrightTests
```
