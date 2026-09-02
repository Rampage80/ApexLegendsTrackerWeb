---
applyTo: "**/*Tests.cs"
---

# Test Guidance

- Keep tests deterministic and focused on one behavior.
- Reuse the existing fixture and test framework conventions.
- Assert the public result or state, not private implementation details.
- Run the focused test first, then widen to the project build when the change warrants it.
