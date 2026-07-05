# Git Workflow

## Branching Strategy
We use a feature-branching workflow (similar to GitFlow but simplified for Unity development).
- `main`: Production-ready code. Must always be stable and buildable.
- `development`: The active integration branch. Features merge here first.
- `feature/<feature-name>`: Branches for new features (e.g., `feature/road-placement`).
- `bugfix/<bug-name>`: Branches for fixing bugs (e.g., `bugfix/save-system-crash`).
- `hotfix/<issue>`: Critical fixes directly applied to `main`.

## Commit Naming Convention
Commits must be clear and descriptive. We follow standard Conventional Commits:
- `feat: [Description]` - For new features.
- `fix: [Description]` - For bug fixes.
- `docs: [Description]` - For documentation changes.
- `refactor: [Description]` - For code refactoring without changing logic.
- `chore: [Description]` - For project setup, tooling, and meta-changes.

*Example:* `feat: Implement GameManager state machine`

## Version Naming Convention
We use Semantic Versioning (SemVer): `MAJOR.MINOR.PATCH`
- **MAJOR**: Incompatible API changes or massive milestone releases (e.g., 1.0.0 for full release).
- **MINOR**: Add functionality in a backwards-compatible manner (e.g., 0.1.0 for Phase 0.1 complete).
- **PATCH**: Backwards-compatible bug fixes.
