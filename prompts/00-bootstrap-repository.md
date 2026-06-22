# Prompt 00 — Bootstrap the Repository

Use this prompt in Codex after placing `AGENTS.md`, `.agents/`, `.codex/`, `docs/codex/`, and `prompts/` in the empty repository.

```text
Read AGENTS.md and the repo skill enterprise-claims-solution-architecture.

Bootstrap the initial repository for `dotnet-azure-devops-reference-architecture`, an Enterprise Claims Processing Platform for the insurance domain.

Create only the foundation files needed for Release 1 planning. Do not implement all services yet.

Requirements:
1. Create a professional README.md with project purpose, architecture goals, tech stack, and release roadmap.
2. Create the initial docs folder with:
   - docs/architecture-overview.md
   - docs/security-architecture.md
   - docs/observability.md
   - docs/ci-cd-strategy.md
   - docs/deployment-architecture.md
   - docs/adr/0001-use-enterprise-claims-domain.md
3. Add Mermaid diagrams where useful.
4. Add a proposed repository structure section.
5. Add a clear section explaining how this project demonstrates agentic AI-assisted development with Codex.
6. Do not create application code yet unless needed for placeholders.

Before editing, provide a short plan. After editing, summarize files changed and next recommended prompt.
```
