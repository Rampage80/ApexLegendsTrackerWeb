---
applyTo: "**/*.cs"
---

# C# Guidance

- Follow existing dependency injection, async, nullability, and exception-handling patterns.
- Keep HTTP behavior in services and keep UI state transitions out of the transport client.
- Use cancellation tokens when the surrounding API already supports them.
- Add or update focused unit tests for changed parsing, request construction, or state behavior; target at least 80% coverage of changed code.
- Generate structured logs at the appropriate `Trace`, `Debug`, `Information`, `Warning`, and `Error` levels, and instrument runtime paths with telemetry. Never log credentials or player-sensitive payloads.
