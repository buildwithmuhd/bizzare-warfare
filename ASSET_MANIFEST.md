# Bizzare Warfare asset manifest

Audit date: 2026-11-14

## Licensed, Godot-ready assets

| Asset set | Local path | Category | Formats | Intended use | Godot status | Licence |
| --- | --- | --- | --- | --- | --- | --- |
| Sci-Fi Essentials Kit (standard) | `Assets/sci-fi/` | Weapons, enemies, props, textures | glTF + BIN, FBX, OBJ, PNG | Prototype weapon display, enemy placeholders, arena dressing | Use the glTF variants; imported by Godot | CC0 1.0; licence in `Assets/sci-fi/License_Standard.txt` |
| Universal Base Characters Kit (standard) | `Assets/Universal Base Characters[Standard]/` | Player/bot character meshes, hairstyles, textures | glTF + BIN, FBX, PNG | Third-person player/bot meshes and future lobby presentation | Godot-specific glTF variants are available | CC0 1.0; licence in `Assets/Universal Base Characters[Standard]/License_Standard.txt` |
| Universal Animation Library (standard) | `Assets/Universal Animation Library[Standard]/` | Character animation library | GLB, FBX, PNG | Locomotion and later bot/player animation integration | Use `Unreal-Godot/UAL1_Standard.glb` (non-root-motion) unless root motion is deliberately required | CC0 1.0; licence in `Assets/Universal Animation Library[Standard]/License.txt` |

## Asset inventory and decisions

| Category | Available | Assessment |
| --- | --- | --- |
| Player character | Two full-body superhero meshes | Usable for third-person body/lobby preview. First-person arms are not supplied; use a temporary procedural weapon presentation until dedicated FP arms are selected. |
| Bot characters | Three sci-fi enemy models plus the base characters | Usable placeholders. The enemy models must be checked for skeleton/animation compatibility before animation retargeting. |
| Weapons | Pistol, revolver, rifle and sniper meshes | Usable as static first-person prototype models; no authored weapon animations were found. |
| Character animation | Universal Animation Library GLB pair | Usable, but retargeting to the selected character skeleton must be verified in Godot before integration. |
| Map/environment | Crates, barrels, desks, lockers, shelves, props, trim textures | Enough for greybox dressing; no complete map/building kit was found. |
| Textures/materials | Base-color, normal, ORM, and selected emissive textures | Usable with imported mesh materials; inspect normal-map import settings at integration time. |
| Audio | None found | Missing: use silence/placeholders until CC0 sound effects are selected. |
| UI/fonts | None found | Missing: start with Godot Controls and default font. |
| VFX | None found | Missing: start with procedural OmniLight/GPUParticles placeholders. |

## Required follow-up

1. Install the matching Godot 4.7.2 .NET editor before creating C# scripts or scenes.
2. Keep the vendor asset directories intact; do not duplicate FBX/OBJ assets into runtime folders. Reference the Godot-ready glTF/GLB files directly.
3. Verify skeleton compatibility between the selected player mesh and the animation library during the character-integration milestone.
4. Source CC0 audio and (optionally) first-person weapon-animation assets later; none are needed to begin a primitive-map milestone.
