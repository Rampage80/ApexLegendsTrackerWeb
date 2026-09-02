# ApexLegendsTracker Web

- This repository is the Blazor WebAssembly frontend. Keep changes focused on the requested behavior.
- Preserve existing public APIs and the shared `ApexLegendsTracker.Shared` contract unless a contract change is explicitly requested.
- Inspect the nearest implementation, caller, and test before editing.
- Prefer the smallest change that follows an existing project pattern.
- Validate with the narrowest relevant test or `dotnet build`; report validation and unresolved risks briefly.
- Do not modify generated output under `bin/` or `obj/`.
- For API client, DTO, URL, serialization, CORS, or authentication work, read `docs/api-contract.md` and check the backend repository before changing the contract.
- Treat `docs/product-roadmap.md` and `docs/cloud-native-roadmap.md` as planning context, not runtime requirements.
