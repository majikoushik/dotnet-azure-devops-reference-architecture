# Security Rules

## Non-Negotiables

- No real secrets in the repository.
- No real customer, policy, claim, or document data.
- No connection strings committed to source control.
- No tokens in pipeline YAML.
- No logging of sensitive claim data.

## Authentication and Authorization

Use a demo-friendly JWT implementation first, but document the production design with Microsoft Entra ID / Managed Identity where appropriate.

Suggested demo roles:

- `Customer`
- `ClaimProcessor`
- `Supervisor`
- `Admin`

## Secret Management

Local development:

- user secrets or environment variables
- `.env.example` only for placeholder keys

Production design:

- Azure Key Vault
- Managed Identity
- secure pipeline variables or variable groups

## Security Checks for Codex

When changing security-related code, Codex should check:

- Are authorization policies applied?
- Are inputs validated?
- Are secrets kept out of source control?
- Are errors safe and non-leaky?
- Are logs free from sensitive data?
- Are pipeline and Bicep files using placeholders instead of real values?
