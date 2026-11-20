# Development log

## 2026-11-14 — Environment and asset audit

### Completed

- Located the project at `C:\Users\Aizen\Documents\bizzare-warfare`.
- Verified Godot 4.7.2 standard editor opens and imports the project assets.
- Verified .NET SDKs 10.0.203 and 11.0.100-rc are installed.
- Audited the existing asset directories and recorded the results in the root `ASSET_MANIFEST.md`.
- Verified Quaternius CC0 1.0 licence files for the Sci-Fi Essentials Kit, Universal Base Characters Kit, and Universal Animation Library.

### Blocker

The installed editor is `Godot_v4.7.2-stable_win64.exe`, the standard build. It does not provide C# support, and the project has no `.csproj`, `.sln`, `.cs`, `.gd`, or `.tscn` files. The C# implementation milestone must wait for the matching Godot 4.7.2 .NET editor.

### Test results

- `Godot --headless --version`: `4.7.2.stable.official.ed1daf0bf`.
- Project asset import metadata is present under `.godot` and beside source assets.
- No runnable main scene is configured.

### Next milestone

Install and launch the Godot 4.7.2 .NET Windows x86_64 editor, then create the C# project, main scene, player scene, and controller.

## 2026-11-14 — Milestones 1–2 implementation

### Implemented

- Added `Scenes/Main/Main.tscn`: a lit, enclosed primitive arena with floor, four collision walls, cover blocks, HUD crosshair, and controls hint.
- Added `Scenes/Player/Player.tscn`: `CharacterBody3D` player with capsule collider and first-person camera.
- Added `Scripts/Player/FpsPlayer.cs`: C# WASD movement, mouse-look, gravity, jump, sprint, collision movement, and mouse capture/release.
- Added `bizzare-warfare.csproj` targeting Godot .NET SDK 4.7.2 and .NET 8.
- Set `Scenes/Main/Main.tscn` as the project main scene.

### Build status

- Godot .NET 4.7.2 editor successfully scanned the project.
- The initial `dotnet build` is blocked by a stale machine NuGet fallback-package path referring to a missing Visual Studio directory. A project-local NuGet configuration has been added to clear that fallback before rebuilding.

### Verification

- Rebuilt after adding `NuGet.Config`: restore completed and `dotnet build` completed with **0 warnings and 0 errors**.
- Launched the project using `Godot_v4.7.2-stable_mono_win64.exe`.
- Confirmed the `bizzare-warfare (DEBUG)` game window is live alongside the Godot editor; no startup or scene-load error was reported.

### Next milestone

Implement the raycast weapon, ammo/reload state, and placeholder muzzle effect as Milestone 3.

## 2026-11-14 — Milestone 3 implementation and verification attempt

### Implemented

- Added `Scenes/Weapons/Rifle.tscn` and attached it beneath the first-person camera in `Scenes/Player/Player.tscn`.
- Added reusable `WeaponBase`, `Rifle`, `IDamageable`, and `Damageable` C# types under `Scripts/Weapons/`.
- Added `TestTarget.cs` and an in-arena 100-health target to exercise the damage contract before bot AI exists.
- Added signal-driven `AmmoHud.cs`, plus a responsive bottom-right ammunition readout.
- Added left-mouse fire and `R` reload actions without altering the existing movement controls.
- Integrated the existing CC0 Quaternius `Assets/sci-fi/glTF/Gun_Rifle.gltf` asset. No gunshot/reload audio or authored muzzle/impact assets were present, so the rifle uses procedural light/mesh effects.
- Corrected the initial oversized rifle transform and replaced fixed ammunition-HUD offsets with bottom-right anchors after visual inspection showed clipping.

### Build verification

- `dotnet build bizzare-warfare.csproj --nologo` passed after the final correction: **0 warnings, 0 errors**.
- The .NET SDK emits its informational preview-SDK message, but it is not counted as a build warning.

### Runtime verification

- Passed: the standalone `bizzare-warfare (DEBUG)` window launched, the arena rendered, the crosshair and initial test target were visible, and the first implementation's ammunition HUD was present.
- Fixed after that inspection: weapon scale/placement and ammunition-HUD anchoring.
- Blocked: after rebuilding the visual correction, the Windows Computer Use helper repeatedly timed out while listing/capturing native windows, including one retry after a session reset. This prevented safe live input injection and debugger inspection.
- Therefore the following are **unverified**, not claimed as passing: corrected rifle appearance, fire input, automatic-fire rate, hitscan hit/impact, target health decrease/death, reload behavior, empty-magazine behavior, and final runtime console state.

### Remaining issue

Milestone 3 remains in progress until the listed runtime checks can be executed in a functioning desktop-control session. The code compiles, but compilation does not substitute for the required gameplay verification.

## 2026-11-14 — Milestone 3 alternative runtime verification

### Inspection and defect fixed

- Re-inspected the player, weapon, damage, test-target, HUD, rifle scene, and project configuration. The project is not a Git repository, so there is no Git status to preserve.
- Found a real hitscan integration defect: `Rifle` searches the hit collider and its parents for `IDamageable`, while the arena `TestTarget` originally kept its `Damageable` component as a child. The target therefore could not receive hitscan damage.
- Fixed the smallest relevant connection: `TestTarget` now implements `IDamageable` and delegates to its existing child `Damageable` component.

### Automated runtime verification

- Added the retained, isolated headless test scene `Scenes/Tests/WeaponVerification.tscn` with `Scripts/Tests/WeaponVerification.cs`.
- Executed it directly through `Godot_v4.7.2-stable_mono_win64_console.exe --headless --path . --scene res://Scenes/Tests/WeaponVerification.tscn`.
- Result: `WEAPON_VERIFICATION: PASS (0 failures)`.
- Verified: one-shot ammunition consumption; shot signal; physics hitscan damage through `IDamageable`; HUD signal update; impact instantiation; held automatic fire at the configured 10 shots/sec limit; release stops fire; reload state; magazine/refill and reserve transfer; full-magazine reload no-op; empty magazine fire prevention; empty-reserve reload no-op.

### Build and direct-launch results

- Final `dotnet build bizzare-warfare.csproj --no-restore --nologo`: **0 warnings, 0 errors**.
- Added `NuGetAudit=false` to suppress an external NuGet vulnerability-feed connectivity warning that is unrelated to project compilation.
- Direct headless launch succeeded and ran the test scene to completion with exit status 0.

### Rendering and environment notes

- Renderer configuration remains `gl_compatibility`; it was not changed.
- The observed ANGLE message is a compatibility fallback for the Intel graphics driver, not a weapon failure. The prior live arena screenshot rendered successfully.
- Headless Godot reports inability to access the local certificate store and `user://logs` directory in the constrained execution environment. These are environment-level messages; the test run reports no weapon-script errors.

### Visual verification status

- The pre-correction running-game screenshot established that the arena, target, crosshair, and initial HUD render, but also revealed rifle/HUD presentation issues that were then fixed.
- The final post-fix visual check is still **unverified**: the native desktop-control helper timed out again before it could enumerate or capture the running game. Automated tests cannot replace this visual requirement.

### Milestone status

Milestone 3 is build-verified and automated-runtime-verified, but remains in progress pending a post-fix visual gameplay capture of the rifle and HUD.

## 2026-11-14 — Milestone 4 controller / gamepad support

### Implemented

- Added `Scripts/Core/InputManager.cs`, a reusable input service that owns the keyboard/mouse and gamepad action mappings, controller deadzones, controller look sensitivity, active-input tracking, and persisted controller preferences at `user://input_settings.cfg`.
- Added gamepad mappings through Godot's Input Map: left stick for movement, right stick for look, A/Cross for jump, left-stick click for sprint, right trigger for the existing shared `fire` action, X/Square for reload, and Start/Options for pause/cursor release.
- Updated `Scripts/Player/FpsPlayer.cs` to use `InputManager` for analog movement and frame-time-scaled right-stick look while preserving existing mouse look, mouse capture/release, WASD, shift sprint, and space jump behavior.
- Added `InputManager` to `Scenes/Player/Player.tscn`. The rifle remains attached to the first-person camera and continues to receive its existing `fire` and `reload` actions, so no weapon-system rewrite was necessary.
- Added the retained headless verification scene `Scenes/Tests/ControllerVerification.tscn` with `Scripts/Tests/ControllerVerification.cs`.

### Automated verification

- `dotnet build bizzare-warfare.csproj --no-restore --nologo`: **0 warnings, 0 errors**.
- `ControllerVerification.tscn` run via the Godot 4.7.2 .NET console executable: `CONTROLLER_VERIFICATION: PASS (0 failures)`.
- Verified mappings for both sticks, right trigger, A/Cross, left-stick click, X/Square, and Start/Options.
- Verified radial deadzones, partial analog movement, normalized diagonal movement, controller/keyboard active-source switching, and right-trigger activation of the rifle's shared `fire` action.
- Headless main-scene smoke launch completed without project script or scene-load errors.

### Runtime limitation

- No physical controller was available to this environment, so real-device stick feel, platform-specific button labels, and live controller/gameplay behavior remain unverified. The headless Godot host also cannot write `user://logs` or read the local certificate store; those environment messages occurred before the test and are unrelated to project scripts.
- The project currently has no menu or pause screen to navigate. `pause` is nevertheless mapped and releases the mouse cursor, and `InputManager.ActiveInputChanged` provides a reusable foundation for controller-aware UI navigation when those screens are added.
