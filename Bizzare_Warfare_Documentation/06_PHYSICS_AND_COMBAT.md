# 06 — Physics & Combat
## Architecture
Input request → weapon state validation → host fire validation → hit detection → damage calculation/application → elimination processing → replicated state and effects.

## Weapon data
ID, display name, base damage, fire rate, magazine size, reload duration, range, spread, recoil and optional hit-zone multiplier. Store values in configuration, not hardcoded logic.

## Initial firing model
Hitscan via raycast. Authority checks ammo, cooldown and plausible firing state, determines hit and applies damage. Client cannot declare a confirmed kill.

## Damage model
`FinalDamage = BaseDamage × ApplicableMultiplier`. Define hit zones only if useful and testable.

## Collision
Separate collision layers/masks for world, player, bot, weapon, projectile (if later added) and interaction objects. Use simple collision geometry and avoid irrelevant raycast targets.

## Feedback
Muzzle flash, firing sound, hit indicator, elimination/death feedback, respawn cue and ammo display. Presentation effects must not control authoritative game state.
