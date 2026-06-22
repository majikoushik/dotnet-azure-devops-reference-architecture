# Prompt Playbook

Use these prompts sequentially in Codex:

1. `00-bootstrap-repository.md`
2. `01-release-1-foundation.md`
3. `02-release-2-data-and-messaging.md`
4. `03-release-3-security.md`
5. `04-release-4-observability.md`
6. `05-release-5-bicep-infrastructure.md`
7. `06-release-6-azure-devops-pipelines.md`
8. `07-release-7-architecture-polish.md`

Recommended workflow:

1. Start a new branch per release.
2. Paste the release prompt into Codex.
3. Review the plan before implementation.
4. Let Codex implement the slice.
5. Run validation.
6. Ask Codex to review its own diff.
7. Commit only after manual review.
