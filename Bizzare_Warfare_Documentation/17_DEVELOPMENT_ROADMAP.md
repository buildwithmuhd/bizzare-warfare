# 17 — Development Roadmap
Milestones are deliverable-based; estimates should be assigned only after the team and available hours are known.

## Phase 1 — Foundation
Configure Godot .NET and SDK, Git, directories, input actions, main scene and test map. **Exit:** project builds and runs.

## Phase 2 — Player
Camera, mouse look, walk/sprint/jump, collision and health. **Exit:** playable controller in graybox.

## Phase 3 — Combat
Weapon framework, hitscan, ammo, reload, damage, death and respawn. **Exit:** complete local combat loop.

## Phase 4 — Match
Spawning, timer, score, scoreboard, win condition and results. **Exit:** local match completes.

## Phase 5 — Bots
Navigation, perception, target selection, attack/reload/respawn and difficulty. **Exit:** bots independently play a match.

## Phase 6 — LAN
Host/join, discovery/direct IP, authoritative validation, state synchronization and disconnect handling. **Exit:** two or more machines complete a match.

## Phase 7 — Content
Initial maps, models, animations, audio, effects, HUD and menu polish. **Exit:** recognizable, content-complete game.

## Phase 8 — QA & optimization
Profile, fix defects, test networking/platforms, verify licenses. **Exit:** release candidate passes acceptance criteria.

## Phase 9 — Release
Package, version, document and distribute. **Exit:** stable initial release.

## Priority
Player movement → combat → match lifecycle → early networking prototype → bots → content → UI polish → optimization/release. Keep networking architecture decoupled from core gameplay.
