# 11 — Data Specification
## Categories
Game config, player settings, weapon configs, bot configs, map configs, transient match state and local statistics.

## Example player settings
```json
{
  "schema_version": 1,
  "audio": {"master_volume": 1.0, "music_volume": 0.7, "effects_volume": 0.9},
  "graphics": {"resolution": "1280x720", "fullscreen": false, "vsync": false, "quality": "low"},
  "controls": {"sensitivity": 1.0, "invert_y": false}
}
```

## Example weapon configuration
```json
{
  "id": "rifle_01",
  "name": "Assault Rifle",
  "damage": 25,
  "fire_rate": 8,
  "magazine_size": 30,
  "reload_duration": 2.0,
  "range": 100,
  "spread": 0.02
}
```
Numbers are examples for prototyping, not approved balance values.

## Persistence and validation
Persist local settings in Godot user data path, not install directory. Validate ranges and resource identifiers. Invalid values should fall back safely. Include schema version and migration/reset strategy for future changes.
