# Web API Contract

This is the Web repository's concise reference for AI-assisted API changes. The backend implementation and shared package remain authoritative.

- Route: `GET /api/v1/players/{platform}/{playerName}`
- Valid platforms: `PC`, `PS4`, `X1`
- The Web client URL-encodes both route values.
- Success responses deserialize to `ApexLegendsTracker.Shared.PlayerLookupResult`.
- The current shared package version consumed by the Web project is `1.1.0`.
- The result uses the structured `Global`, `Realtime`, and `Legends` fields; do not reintroduce `RawJson` or fabricate statistics.
- Contract changes require coordinated updates to the backend, shared package, Web client, configuration, and relevant tests.
- Local API URL is configured in `wwwroot/appsettings.json`; production uses `wwwroot/appsettings.Production.json`.
