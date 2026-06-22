# Codex Agent Guidance

This document extends `AGENTS.md` with workflow details. Keep `AGENTS.md` concise and put heavier guidance here.

## Operating Mode

Codex should work in small slices:

1. Understand the request.
2. State assumptions.
3. Plan the smallest useful change.
4. Implement.
5. Validate.
6. Review the diff.
7. Update documentation.
8. Summarize risks and next steps.

## Architecture-First Development

For this repository, code is not enough. A change is stronger when it also explains:

- Why the design exists.
- Which Azure services it maps to.
- How it is secured.
- How it is observed.
- How it is deployed.
- How it scales.
- What trade-offs were considered.

## Avoid These Agent Mistakes

- Do not create a large monolith while calling it microservices.
- Do not add many packages without need.
- Do not generate fake secrets.
- Do not hard-code Azure subscription details.
- Do not create production-grade claims complexity too early.
- Do not ignore tests and documentation.
- Do not replace architecture decisions with vague statements.

## Preferred Response Pattern

Codex final response should include:

```md
## Summary

## Files Changed

## Validation

## Architecture / Security Impact

## Follow-ups
```
