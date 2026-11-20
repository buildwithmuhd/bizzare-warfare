# 01 — Product Requirements Document
## Vision
Deliver a lightweight, responsive FPS that is easy to launch and play with friends over a local network, with bots available for filling matches or solo practice.

## Audience
Casual FPS players, LAN groups, players with modest desktop hardware, and users who prefer small downloadable games.

## Core loop
Launch → choose Host/Join → configure lobby → load arena → fight players/bots → match ends → scoreboard → rematch/menu.

## Functional requirements
- FR-01: First-person movement, camera and collision.
- FR-02: Weapon firing, reload, ammo and damage.
- FR-03: LAN host and client connection.
- FR-04: AI bots with configurable difficulty.
- FR-05: Match timer, score, deaths, elimination and respawn.
- FR-06: Scoreboard, HUD, lobby and results screen.
- FR-07: Multiple maps and spawn points.
- FR-08: Graphics, audio and control settings persist locally.
- FR-09: Handle connection errors and disconnects with clear feedback.

## Non-functional requirements
- Target 60 FPS at proposed 720p Low baseline, pending hardware validation.
- Stable match state and authoritative host-side combat validation.
- Modular, testable C# systems and configurable game values.
- Menus readable and keyboard/mouse navigable.

## V1 scope
Included: FFA Deathmatch, LAN listen-server, bots, initial maps, one firearm and knife baseline, settings, scoreboard, desktop builds.
Excluded: ranked matchmaking, accounts, cloud saves, monetization, dedicated servers, global matchmaking, progression/battle pass and host migration.
