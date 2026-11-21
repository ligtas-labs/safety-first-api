# Safety First API agent instructions

Read [the PRD](docs/PRD.md), [the design](docs/DESIGN.md), [the contribution workflow](CONTRIBUTING.md), and the linked Linear ticket when one exists. Use the commands in the [README](README.md).

Keep controllers and workflow-specific types under their feature. Validate route, authorized actor, denied actor and synthetic persistence behavior. Keep Program.cs focused on startup and registrations.

Use synthetic fixtures. Include requirement IDs, applicable verification results and unexecuted checks in the PR. Keep credentials and participant data out of code, logs and review artifacts. Pending safety content, scoring, research and integration decisions require the appropriate owner before implementation.
