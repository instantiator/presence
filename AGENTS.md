<!-- dev-qual:start -->

# Agent instructions

These rules are mandatory and override your defaults. `dev-qual/` is in this repo (or clone <https://github.com/instantiator/dev-qual> to a temporary location once per session).

## Non-negotiable rules

1. Less is more: concise, readable code and simple interfaces. No unrequested abstractions.
2. Comments state intent, not mechanics. Comment classes, functions, and consts; skip trivial ones.
3. Zero compiler errors and warnings; zero linter errors and warnings.
4. Never cast to `any` (or equivalent type escapes), especially in test mocks.
5. Never store secrets in the code base.
6. Only change code you have been asked to change; ask permission otherwise.

## Process

- Before starting work: run `dev-qual/scripts/check-updates.sh`; if it reports an update, offer to run `dev-qual/scripts/upgrade.sh` first.
- Planning is collaborative: present options and trade-offs before committing to libraries, approaches, data structures, or key business logic. Plan with the `plan-work` skill (`dev-qual/skills/plan-work/SKILL.md`).
- Before coding: read `dev-qual/guidance/process/before-coding.md`.
- Route via `dev-qual/guidance/index.md`: lazy-load only the docs relevant to the task, following references recursively when needed.
- For multi-step tasks (setup, review, deploy, audit): use the matching skill from `dev-qual/skills/index.md`.
- After every change: run `dev-qual/scripts/check.sh` and fix what it reports.
- When done: apply the judgment items in `dev-qual/guidance/process/after-coding.md` — tests for intent and edge cases, simplification, comment accuracy, documentation updates.
- Anything left undone goes in `docs/outstanding-issues.md` with a measurable condition for when to act on it, and into your summary to the user.
- Sub-agents and a choice of model available? Allocate work by capability and cost first: `dev-qual/guidance/process/model-allocation.md`.

## Assurance

- State the filename of any guidance doc you read, so the user can see you are following it.

<!-- dev-qual:end -->

<!-- dev-qual:skills:start -->

## Skills (multi-step task playbooks)

Match the task against a trigger below, then follow that SKILL.md literally.

- [plan-work](dev-qual/skills/plan-work/SKILL.md) — Build a staged, gated implementation plan before coding. Use when asked to plan, in planning mode, or before any task with more than one unit of work.
- [project-setup](dev-qual/skills/project-setup/SKILL.md) — Set up a new project, with or without a templating tool. Use when starting a project, scaffolding an app, or working in an empty repo.
- [ci-setup](dev-qual/skills/ci-setup/SKILL.md) — Set up continuous integration for a repository. Use when asked to add CI, or when a repo has no workflow files.
- [testing-setup](dev-qual/skills/testing-setup/SKILL.md) — Add test suites and per-suite launch scripts to a project. Use when a project lacks tests or needs a new test tier.
- [quality-review](dev-qual/skills/quality-review/SKILL.md) — Review and repair code quality after changes. Use after completing coding work, when asked to review code, or when check.sh fails.
- [docs-review](dev-qual/skills/docs-review/SKILL.md) — Review and update project documentation against the code. Use after a feature lands, or when docs may be stale.
- [deploy](dev-qual/skills/deploy/SKILL.md) — Run or deploy the application, locally or remotely. Use when asked to deploy, release, or run the app.
- [toolchain-setup](dev-qual/skills/toolchain-setup/SKILL.md) — Set up or repair linters, formatters, and typechecking, consistent across CLI and IDE. Use when configuring tooling or when CLI and editor disagree.
- [deps-audit](dev-qual/skills/deps-audit/SKILL.md) — Audit, update, and resolve conflicts in dependencies. Use for dependency updates, vulnerability alerts, or version conflicts.
- [prereqs](dev-qual/skills/prereqs/SKILL.md) — Check and install the tools a project needs. Use on a fresh machine, at onboarding, or when a required tool is missing.
- [essential-behaviours](dev-qual/skills/essential-behaviours/SKILL.md) — Install the enforcement that makes key behaviours automatic. Use at project onboarding, or when asked to make sure something always happens.
- [adr](dev-qual/skills/adr/SKILL.md) — Think through and record an architectural decision. Use when making architecture, infrastructure, or significant design choices.
- [update-latest](dev-qual/skills/update-latest/SKILL.md) — Bring an installed dev-qual (project or user scope) up to date without losing customisations. Use when an update is reported, the dev-qual checkout has moved on, or setup looks stale.

<!-- dev-qual:skills:end -->
