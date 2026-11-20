# Prototype roadmap

| Milestone | Status | Notes |
| --- | --- | --- |
| Environment and asset audit | Complete | Godot-ready CC0 model/texture/animation assets identified. |
| Godot 4 .NET editor | Blocked | Standard Godot 4.7.2 build is installed; matching .NET editor is required for C#. |
| 1. Main game scene | Complete | Lit collision arena, spawn point, first-person camera, crosshair, and controls hint verified in the running game. |
| 2. Player controller | Complete | C# `CharacterBody3D`: WASD, mouse look, jump, gravity, sprint, mouse capture/release, and collision movement. |
| 3. First weapon | In progress — visual verification blocked | Build and headless gameplay verification pass (hitscan/damage, ammo, automatic fire-rate, reload, empty-magazine, HUD signals, and effects). Post-fix on-screen rifle/HUD capture remains blocked by native desktop-control timeouts. |
| 4. Controller / gamepad support | Complete — automated verification | Centralized mappings, analog movement/look, deadzones, settings persistence, controller/keyboard switching, right-trigger weapon firing, and headless verification pass. Physical-device testing remains pending. |
| 5. Health and damage | Pending | Extend the existing damage interface with death and respawn. |
| 6. HUD | Pending | Expand the responsive Control-based HUD. |
| 7. Functional bot | Pending | Navigation, combat, respawn. |
| 8. Deathmatch | Pending | Scoring, timer, match end, scoreboard. |
| 9. Menu and lobby | Pending | LAN UI only after core gameplay. |
| 10. LAN multiplayer | Pending | Server-authoritative implementation after a stable local prototype. |
