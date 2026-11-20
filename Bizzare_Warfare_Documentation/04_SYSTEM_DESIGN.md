# 04 — System Design
## Runtime lifecycle
Launch → initialize core → main menu → host/join → establish session → load map → spawn humans/bots → initialize scores/timer → gameplay → evaluate match condition → results → rematch/menu.

## Match initialization responsibilities
1. Validate match configuration and session.
2. Load selected map and spawn configuration.
3. Spawn connected players and configured bots.
4. Initialize scores, timer and match state.
5. Replicate match initialization.
6. Begin gameplay.

## Authority ownership
Host/server owns match state, health, damage, score, ammo, fire-rate validation, bot decisions and spawn assignment. Clients own input collection and local presentation.

## Failure handling
Handle discovery failure, invalid IP, full lobby, timeout, disconnect, missing map/resource, invalid settings and unexpected match end. Return actionable messages and recover to a safe screen where possible.
