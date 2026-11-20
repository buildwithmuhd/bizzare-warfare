# 03 — Software Architecture
## Approach
Modular, component-oriented architecture using Godot scenes/nodes and C# scripts. Keep gameplay rules independent from UI presentation and network transport details.

## Suggested directories
```text
Assets/{Characters,Weapons,Maps,Materials,Textures,Audio,Animations,UI}
Scenes/{Main,Player,Weapons,Bots,Maps,UI,Effects}
Scripts/{Core,Player,Combat,Weapons,Bots,Networking,Match,UI,Settings,Utilities}
Data/{Weapons,Bots,Maps,Config}
Tests/{Unit,Integration,Multiplayer}
Documentation/
```

## Modules and responsibilities
- GameManager: application/game lifecycle.
- MatchManager: start/end and match conditions.
- PlayerController: movement and camera.
- PlayerCombat / WeaponManager: combat state and weapon handling.
- DamageSystem: authority-validated damage.
- BotController / BotManager: AI and bot lifecycle.
- NetworkManager / LobbyManager: session and lobby.
- ScoreManager: score/death records.
- UIManager: navigation and screen presentation.
- SettingsManager / AudioManager: persistence and sound.

## Rules
1. Keep modules cohesive and dependencies explicit.
2. Use signals/events for loose coupling.
3. Never put game rules inside UI scripts.
4. Keep authority checks in gameplay/network logic.
5. Prefer data-driven parameters.
6. Avoid premature abstraction and unnecessary frameworks.

## Example contract
```csharp
public interface IDamageable
{
    void ApplyDamage(float amount, ulong attackerId);
}
```
