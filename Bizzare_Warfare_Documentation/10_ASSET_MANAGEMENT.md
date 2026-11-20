# 10 — Asset Management
## Art direction (proposal)
Low-poly models, clean silhouettes, readable character shapes, simple materials, contrast and optimized environments.

## Asset categories
Characters, weapons, environment props, materials, textures, audio, animation and UI.

## Candidate sources
Kenney (kenney.nl/assets), Quaternius (quaternius.com), Poly Haven (polyhaven.com), OpenGameArt (opengameart.org), Freesound (freesound.org). Verify each asset's individual license and attribution requirements; do not assume platform-wide licensing.

## Naming convention
`CHR_Player_Default`, `WPN_Rifle_01`, `MAP_Industrial_Arena`, `SFX_Rifle_Fire`, `MAT_Concrete`, `TEX_Concrete_Albedo`.

## Optimization
Reasonable polygon counts, texture compression, material reuse, controlled texture sizes, LOD where justified, draw-call reduction and visibility optimization.

## Manifest
Track asset ID/name, category, source URL, author, license, attribution, modification status, local path and acquisition date. Do not ship assets with unclear rights.
