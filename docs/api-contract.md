# Web API Contract

This is the Web repository's concise reference for AI-assisted API changes. The backend implementation and shared package remain authoritative.

- Route: `GET /api/v1/players/{platform}/{playerName}`
- Route: `GET /api/v1/map-rotation?version=1|2`
- Route: `GET /api/v1/predator-thresholds`
- Route: `POST /api/v1/chat`
- Valid platforms: `PC`, `PS4`, `X1`
- The Web client URL-encodes both route values.
- Success responses deserialize to `ApexLegendsTracker.Shared.PlayerLookupResult`.
- The current shared package version consumed by the Web project is `1.7.0`.
- The result uses the structured `Global`, `Realtime`, and `Legends` fields; do not reintroduce `RawJson` or fabricate statistics.
- `Global` includes rank imagery, account level, and `toNextLevelPercent`; `Legends.Selected.ImgAssets` includes the selected legend icon and banner.
- The results page uses the selected legend banner as the full-width hero background and overlays the player overview and selected legend icon/name on it.
- `Legends.All` contains each character's icon and stat data for the results-page character list.
- The contract intentionally excludes arena, battlepass, badges, and selected-legend game-info sections.
- Contract changes require coordinated updates to the backend, shared package, Web client, configuration, and relevant tests.
- The dashboard intentionally displays only map rotation and Predator thresholds from the live status endpoints. Leaderboards, server health, queue metrics, and fabricated player progression are not dashboard data.
- The dashboard requests both live status endpoints when loaded; the backend shares their responses through a one-minute cache across API instances.
- Local API URL is configured in `wwwroot/appsettings.json`; production uses `wwwroot/appsettings.Production.json`.
- The home page includes a `ChatPanel` component (`Shared/ChatPanel.razor`) that posts `ApexLegendsTracker.Shared.ChatRequest` (`Message` only) to `POST /api/v1/chat` and renders the `ChatResponse.Reply`. `ChatSource` currently only has a `Knowledge` value; there is no player-specific chat path yet (deferred future work, tracked in the shared repo's chat agent plan).
- The client deserializes `ChatResponse` with `PropertyNameCaseInsensitive` and a `JsonStringEnumConverter` since the backend serializes `ChatSource` as a string (e.g. `"Knowledge"`).
- Chat telemetry events (`ChatRequested`/`ChatSucceeded`/`ChatFailed`) never include the raw message text, matching the existing telemetry privacy convention.

