---
applyTo: "**/*.razor"
---

# Razor Guidance

- Preserve the existing Bootstrap-based layout and responsive behavior.
- Keep markup readable and use the shared state/service abstractions already present.
- Display only data backed by the API; label intentional preview data clearly.
- Prefer accessible labels, semantic headings, and keyboard-usable controls.
- Meet WCAG 2.1 for every UI change: use semantic HTML, programmatic labels and accessible names, logical heading/order structure, keyboard operability, visible focus, sufficient color contrast, responsive text/layout, and status/error messaging that is not conveyed by color alone.
- Add or update Playwright accessibility assertions for changed UI behavior when possible; otherwise record the specific WCAG 2.1 criteria checked manually.
- Add or update a Playwright test for every browser-visible workflow or UI behavior change; target at least 80% coverage of changed behavior.
- Instrument meaningful user workflows, client failures, and API interactions with telemetry. Keep diagnostics structured and free of credentials or player-sensitive payloads.
