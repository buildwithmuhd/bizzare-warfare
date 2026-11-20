# 02 — Constraints & Assumptions
## Technology constraints
- Godot 4 .NET; pin exact engine and compatible .NET SDK versions before development.
- C# is the primary language for gameplay, combat, bots, networking, match logic, settings and UI logic.
- GDScript only for justified small editor/scene glue; C++ GDExtension excluded from V1 absent profiling evidence.
- Desktop targets: Windows first, Linux second, macOS after validation. Godot C# web export is not a V1 target.

## Proposed defaults (not approved requirements)
| Parameter | Proposal |
|---|---|
| Human players | 8 maximum |
| Bots | Up to 7 |
| Mode | Free-for-all Deathmatch |
| Match duration | 8 minutes |
| Score target | 20 eliminations |
| Maps | 3 |
| Weapons | Rifle + knife |
| Bot difficulties | Easy / Normal / Hard |
| Performance | 60 FPS at 1280×720 Low |

## Infrastructure and scope
No account, cloud, dedicated-server or always-online requirement. LAN discovery and direct IP join are desired. Feature additions must demonstrate value to the core loop and be explicitly scoped.
