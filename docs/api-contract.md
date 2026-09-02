# Web API Contract

This is the Web repository's concise reference for AI-assisted API changes. The backend implementation and shared package remain authoritative.

- Route: `GET /api/v1/players/{platform}/{playerName}`
- Valid platforms: `PC`, `PS4`, `X1`
- The Web client URL-encodes both route values.
- Success responses deserialize to `ApexLegendsTracker.Shared.PlayerLookupResult`.
- The current shared package version consumed by the Web project is `1.3.0`.
- The result uses the structured `Global`, `Realtime`, and `Legends` fields; do not reintroduce `RawJson` or fabricate statistics.
- `Global` includes rank imagery, account level, and `toNextLevelPercent`; `Legends.Selected.ImgAssets` includes the selected legend icon and banner.
- The results page uses the selected legend banner as the full-width hero background and overlays the player overview and selected legend icon/name on it.
- `Legends.All` contains each character's icon and stat data for the results-page character list.
- The contract intentionally excludes arena, battlepass, badges, and selected-legend game-info sections.
- Contract changes require coordinated updates to the backend, shared package, Web client, configuration, and relevant tests.
- Local API URL is configured in `wwwroot/appsettings.json`; production uses `wwwroot/appsettings.Production.json`.
