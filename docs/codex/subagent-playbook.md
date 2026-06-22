# Optional Subagent Playbook

For complex reviews, ask Codex to spawn specialized subagents and consolidate the results.

Example prompt:

```text
Review the current branch against main. Spawn separate agents for:
1. Architecture and service boundaries
2. Security and secrets handling
3. Test coverage and correctness
4. Azure DevOps pipeline quality
5. Bicep/IaC quality
Wait for all agents, then give me a consolidated review with prioritized fixes.
```

Use this only for complex diffs because multi-agent workflows can consume more tokens.
