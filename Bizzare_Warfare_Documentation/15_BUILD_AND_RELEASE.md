# 15 — Build & Release
## Targets
Windows first; Linux and macOS after validation.

## Development builds
Useful logs, debug tools and test configuration. Avoid production-only restrictions during iteration.

## Release builds
Optimized export, correct resource inclusion, version identifier, clean settings storage and no development-only interfaces.

## Versioning
Semantic versioning `MAJOR.MINOR.PATCH`; examples 0.1.0, 0.2.0, 1.0.0.

## Release checklist
- Resolve blockers.
- Test Windows and LAN across multiple machines.
- Test bots and settings persistence.
- Complete performance benchmark.
- Verify asset licenses.
- Prepare release notes and version.
- Test clean install and launch.

## Distribution
Initial downloadable desktop build. itch.io or Steam may be considered later, subject to readiness and platform requirements.
