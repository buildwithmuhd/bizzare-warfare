# 08 — Bot AI
## Goal
Bots provide useful opponents without omniscient information or impossible aim.

## Architecture
Use a finite-state machine for V1: Idle, Patrol, Investigate, Pursue, Attack, Reload, Retreat, Respawn.

## Perception
Distance, field-of-view and line-of-sight raycasts. Optional later: sound/recent combat event awareness. Bots must not know all player locations by default.

## Navigation
Use Godot navigation meshes. Compute path to target, follow waypoints, re-path when needed and recover when stuck. Avoid full path recalculation every frame.

## State transitions
- Idle → Patrol when match starts.
- Patrol → Pursue when target detected.
- Pursue → Attack when target is in effective range and visible.
- Attack → Reload when magazine empty.
- Attack → Investigate when target lost.
- Attack → Retreat when health is low (if behavior enabled).
- Eliminated → Respawn.

## Difficulty parameters
Easy/Normal/Hard adjust reaction delay, accuracy, target acquisition speed, aggression and tactical movement. Difficulty must not bypass ammo, range or line-of-sight rules.

## Performance
Stagger perception, cap pathfinding frequency, use distance-based update rates and avoid costly per-frame searches.
