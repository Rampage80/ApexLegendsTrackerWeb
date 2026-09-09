# ApexLegendsTracker Web

- This repository is the Blazor WebAssembly frontend. Keep changes focused on the requested behavior.
- Preserve existing public APIs and the shared `ApexLegendsTracker.Shared` contract unless a contract change is explicitly requested.
- Inspect the nearest implementation, caller, and test before editing.
- Prefer the smallest change that follows an existing project pattern.
- Validate with the narrowest relevant test or `dotnet build`; report validation and unresolved risks briefly.
- Every production-code or user-visible behavior change must add or update an automated test. Use unit tests for state and client logic and Playwright tests for browser-visible workflows; target at least 80% coverage of changed code and report the measured result.
- All UI changes must conform to WCAG 2.1, including keyboard access, visible focus, semantic structure, labels, accessible names, sufficient color contrast, responsive text/layout, and non-color-only status communication. Add an automated accessibility assertion or document the manual WCAG check when automation cannot cover the criterion.
- Generated runtime code must include structured logging at the appropriate `Trace`, `Debug`, `Information`, `Warning`, and `Error` levels, without logging secrets or player-sensitive payloads.
- Telemetry is required for every runtime feature and user workflow. Instrument client failures, API calls, and meaningful UI events using the repository's Application Insights/OpenTelemetry conventions; do not generate unobservable code.
- Do not modify generated output under `bin/` or `obj/`.
- For API client, DTO, URL, serialization, CORS, or authentication work, read `docs/api-contract.md` and check the backend repository before changing the contract.
- Treat `docs/product-roadmap.md` and `docs/cloud-native-roadmap.md` as planning context, not runtime requirements.
