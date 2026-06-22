# DevOps Rules

## CI Strategy

CI should eventually include:

1. Restore
2. Build
3. Unit tests
4. Integration tests
5. Architecture tests
6. Code coverage artifact
7. Static analysis placeholder
8. Docker build
9. Container scan placeholder
10. Publish artifact/image

## CD Strategy

CD should eventually include:

1. Validate Bicep
2. Deploy infrastructure
3. Deploy container apps
4. Run smoke tests
5. Verify health endpoints
6. Publish deployment summary

## Pipeline Design Principles

- Use reusable templates.
- Keep environment-specific values in parameters or variable groups.
- Do not hard-code secrets.
- Separate dev/test/prod deployment concerns.
- Prefer approval gates for production-like deployment stages.
- Make the pipeline readable for recruiters.

## Suggested Pipeline Files

```text
pipelines/
  azure-pipelines.yml
  templates/
    build-dotnet.yml
    test-dotnet.yml
    docker-build.yml
    security-scan.yml
    deploy-infra-bicep.yml
    deploy-container-apps.yml
    smoke-test.yml
```
