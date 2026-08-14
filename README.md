# ApexLegendsTracker Web

Blazor WebAssembly frontend for ApexLegendsTracker.

## Prerequisites

- .NET 10 SDK

## Configuration

Set the backend API base URL in:

- `src/ApexLegendsTracker.Web/wwwroot/appsettings.json`

Property:

- `ApiBaseUrl` (default: `http://localhost:5165/`)

## Run

```powershell
dotnet run --project ./src/ApexLegendsTracker.Web
```

## Test

```powershell
dotnet test
```
