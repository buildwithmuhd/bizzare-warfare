# 14 — Testing & QA
## Types
Unit, integration, gameplay, multiplayer, AI, performance, compatibility and regression testing.

## Core test cases
Player moves/jumps/collides/respawns; weapon fires/reloads and obeys cooldown; damage and scores update; host creates session and clients join; multiple clients synchronize; disconnect removes player; bots navigate/attack/reload/respawn; menus and settings work; invalid IP/config yields clear error.

## Release blockers
Crashes in normal play, match cannot finish, clients can trivially forge damage/score, persistent desync, critical feature failure, broken supported-platform install or unlicensed asset.

## Severity
Critical: release-blocking instability/security. High: major gameplay feature broken. Medium: usability/non-critical functionality. Low: cosmetic/minor issue.

Maintain reproducible bug reports with build, platform, steps, expected and actual behavior, logs and severity.
