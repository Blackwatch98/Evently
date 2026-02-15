# Contributing

## Commit Message Guidelines

I do follow the [Conventional Commits](https://www.conventionalcommits.org/en/v1.0.0/) specification for commit messages.

### Commit Types Cheat Sheet

| Type       | Description                                  | Example                                       |
| ---------- | -------------------------------------------- | --------------------------------------------- |
| `feat`     | A new feature                                | `feat: add some functionality`         |
| `fix`      | A bug fix                                    | `fix: resolve memory leak problem` |
| `docs`     | Documentation changes                        | `docs: update method A documentation`             |
| `style`    | Code style changes (formatting, whitespace)  | `style: fix colors on page B` |
| `refactor` | Code restructuring without changing behavior | `refactor: simplify folder hierarchy logic`   |
| `perf`     | Performance improvements                     | `perf: optimize token refresh mechanism`      |
| `test`     | Adding or updating tests                     | `test: add unit tests for method A`      |
| `chore`    | Build/tooling changes, dependency updates    | `chore: update .NET SDK version`              |
| `build`    | Build system or dependency changes           | `build: update NuGet packages`                |
| `ci`       | CI/CD configuration changes                  | `ci: add GitHub Actions workflow`             |

### Breaking Changes

For breaking changes, append `!` after the type/scope:

- `feat!: remove deprecated method`
- `fix(api)!: change authentication flow`

### Format

```
<type>[optional scope]: <description>

[optional body]

[optional footer]
```

For more details, see the [Conventional Commits specification](https://www.conventionalcommits.org/en/v1.0.0/).
