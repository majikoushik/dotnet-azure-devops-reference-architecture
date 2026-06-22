# Codex Review Checklist

Use this checklist before accepting a Codex-generated change.

## Architecture

- Does the change fit the enterprise claims platform story?
- Are service boundaries still clear?
- Is the design too complex for the current release?
- Should an ADR be added or updated?

## Code Quality

- Are names clear?
- Are controllers/endpoints thin?
- Is business logic testable?
- Are DTOs used at boundaries?
- Are async and cancellation tokens used for I/O?

## Security

- Are secrets absent?
- Is authorization enforced where needed?
- Are logs safe?
- Are error messages safe?

## DevOps

- Do pipelines remain reusable and readable?
- Are cloud-changing commands guarded?
- Are environment values parameterized?

## Observability

- Are logs structured?
- Is correlation preserved?
- Are health checks included?
- Is tracing considered?

## Validation

- Were relevant tests run?
- Are failing tests explained?
- Is documentation updated?
