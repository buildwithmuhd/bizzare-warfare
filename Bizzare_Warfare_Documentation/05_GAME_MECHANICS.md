# 05 — Game Mechanics
## Mode
Initial proposed mode: Free-for-all Deathmatch. Every participant competes individually; teams are not part of V1.

## Match rules (proposed)
- One point per elimination; deaths tracked independently.
- Match ends on timer expiry or score target.
- Winner is highest score; tie-break by eliminations, then fewer deaths; unresolved tie is shared.
- Proposed match: 8 minutes / 20 score target.

## Movement
Walking, sprinting, jumping, air control and grounded/collision handling. Crouch/slide are deferred until validated as core to intended feel.

## Player states
Idle, Moving, Sprinting, Jumping, Falling, Attacking, Reloading, Eliminated, Respawning. Prevent conflicting transitions, such as firing during reload where not allowed.

## Spawning and respawn
Map-defined spawn points. Prefer valid points away from immediate enemy visibility and avoid repetitive spawn reuse. Proposed respawn delay: 3 seconds. Temporary protection, if added, must be short and cannot enable exploitative behavior.

## Scoreboard
Display player name, eliminations, deaths, score and ping when available. Visually distinguish bots from humans.
