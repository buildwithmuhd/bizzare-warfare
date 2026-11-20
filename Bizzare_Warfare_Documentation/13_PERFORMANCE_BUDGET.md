# 13 — Performance Budget
## Proposed baseline
1280×720 Low, 60 FPS, Windows desktop; hardware target and player/bot counts require validation.

At 60 FPS, frame budget is approximately 16.67 ms. Budget spans game logic, physics, AI, rendering, audio, networking and UI.

## Optimization
- Rendering: low-poly assets, material reuse, limit costly transparency, visibility culling.
- Physics: simple collision geometry, appropriate layers, limit raycasts and active bodies.
- AI: stagger perception and path updates.
- Networking: send necessary state only, avoid excessive RPCs and tune update rates.
- Memory: avoid repeated loads, release unused resources and pool only where measurements justify it.

## Profiling
Use Godot profiler and platform tools. Measure CPU/GPU frame time, physics, AI, network traffic, memory and loading time. Optimization should be evidence-driven.
