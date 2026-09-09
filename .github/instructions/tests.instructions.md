---
applyTo: "**/*Tests.cs"
---

# Test Guidance

- Keep tests deterministic and focused on one behavior.
- Reuse the existing fixture and test framework conventions.
- Assert the public result or state, not private implementation details.
- Every production-code or user-visible behavior change must have a corresponding automated test. Use unit tests for logic and Playwright tests for browser-visible workflows; target at least 80% coverage of changed code or behavior.
- For UI changes, include WCAG 2.1 checks for keyboard navigation, accessible names/labels, focus behavior, semantic structure, contrast-sensitive states, and non-color-only status or error communication. Use automated accessibility checks where available and record uncovered criteria.
- Verify telemetry and structured logging behavior for important success, dependency, validation, and failure paths without coupling tests to unstable logger formatting.
- Run the focused test first, then widen to the project build when the change warrants it.
