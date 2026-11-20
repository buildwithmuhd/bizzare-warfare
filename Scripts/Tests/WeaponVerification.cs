using Godot;
using BizzareWarfare.UI;
using BizzareWarfare.Weapons;

namespace BizzareWarfare.Tests;

/// <summary>Headless verification for Milestone 3's live physics, ammo, HUD, and damage paths.</summary>
public partial class WeaponVerification : Node3D
{
    private readonly PackedScene _rifleScene = GD.Load<PackedScene>("res://Scenes/Weapons/Rifle.tscn");
    private int _failures;

    public override async void _Ready()
    {
        try
        {
            InputMap.AddAction("fire");
            InputMap.AddAction("reload");
            await VerifyDamageEffectsAndHud();
            await VerifyAutomaticFireAndReload();
            await VerifyEmptyMagazine();
        }
        catch (System.Exception exception)
        {
            Fail($"Unhandled test exception: {exception.Message}");
        }

        GD.Print($"WEAPON_VERIFICATION: {(_failures == 0 ? "PASS" : "FAIL")} ({_failures} failures)");
        GetTree().Quit(_failures == 0 ? 0 : 1);
    }

    private async System.Threading.Tasks.Task VerifyDamageEffectsAndHud()
    {
        var camera = new Camera3D { Current = true };
        AddChild(camera);
        var rifle = _rifleScene.Instantiate<Rifle>();
        camera.AddChild(rifle);

        var target = CreateTarget(new Vector3(0, 0, -5));
        AddChild(target);
        var hud = CreateHud();
        AddChild(hud);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        await ToSignal(GetTree(), SceneTree.SignalName.PhysicsFrame);

        int shotSignals = 0;
        rifle.ShotFired += () => shotSignals++;
        rifle.TryFire();
        await ToSignal(GetTree().CreateTimer(0.02), SceneTreeTimer.SignalName.Timeout);

        Expect(rifle.MagazineAmmo == 29, "one fired round consumes exactly one magazine round");
        Expect(shotSignals == 1, "shot-fired signal is emitted");
        Expect(target.Health.CurrentHealth == 75.0f, "hitscan applies rifle damage through IDamageable");
        Expect(hud.GetNode<Label>("Ammo").Text.Contains("29 / 090"), "ammo HUD receives the weapon signal");
        Expect(GetChildCount() > 3, "impact effect is instantiated after a raycast hit");

        camera.QueueFree();
        target.QueueFree();
        hud.QueueFree();
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    }

    private async System.Threading.Tasks.Task VerifyAutomaticFireAndReload()
    {
        var camera = new Camera3D { Current = true };
        AddChild(camera);
        var rifle = _rifleScene.Instantiate<Rifle>();
        camera.AddChild(rifle);
        int shotSignals = 0;
        rifle.ShotFired += () => shotSignals++;
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        Input.ActionPress("fire");
        await ToSignal(GetTree().CreateTimer(0.36), SceneTreeTimer.SignalName.Timeout);
        Input.ActionRelease("fire");
        int afterHeldFire = shotSignals;
        await ToSignal(GetTree().CreateTimer(0.16), SceneTreeTimer.SignalName.Timeout);

        Expect(afterHeldFire >= 3 && afterHeldFire <= 4, "held fire repeats at the configured 10 shots/second limit");
        Expect(shotSignals == afterHeldFire, "releasing fire stops automatic shots");
        int magazineBeforeReload = rifle.MagazineAmmo;
        rifle.TryReload();
        Expect(rifle.IsReloading, "reload enters the reloading state");
        await ToSignal(GetTree().CreateTimer(rifle.ReloadDuration + 0.08), SceneTreeTimer.SignalName.Timeout);
        int transferred = rifle.MagazineCapacity - magazineBeforeReload;
        Expect(rifle.MagazineAmmo == rifle.MagazineCapacity, "reload refills the magazine to capacity");
        Expect(rifle.ReserveAmmo == rifle.StartingReserveAmmo - transferred, "reload deducts exactly the transferred reserve rounds");
        int reserveAfterReload = rifle.ReserveAmmo;
        rifle.TryReload();
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Expect(!rifle.IsReloading && rifle.ReserveAmmo == reserveAfterReload, "full-magazine reload does not alter ammunition");

        camera.QueueFree();
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
    }

    private async System.Threading.Tasks.Task VerifyEmptyMagazine()
    {
        var camera = new Camera3D { Current = true };
        AddChild(camera);
        var rifle = _rifleScene.Instantiate<Rifle>();
        rifle.MagazineCapacity = 2;
        rifle.StartingReserveAmmo = 0;
        rifle.FireRate = 100.0f;
        camera.AddChild(rifle);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        rifle.TryFire();
        await ToSignal(GetTree().CreateTimer(0.02), SceneTreeTimer.SignalName.Timeout);
        rifle.TryFire();
        await ToSignal(GetTree().CreateTimer(0.02), SceneTreeTimer.SignalName.Timeout);
        int emptyMagazine = rifle.MagazineAmmo;
        rifle.TryFire();
        rifle.TryReload();
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        Expect(emptyMagazine == 0 && rifle.MagazineAmmo == 0, "an empty magazine cannot fire or become negative");
        Expect(!rifle.IsReloading && rifle.ReserveAmmo == 0, "empty-magazine reload with no reserve is ignored");
        camera.QueueFree();
    }

    private static VerificationTarget CreateTarget(Vector3 position)
    {
        var target = new VerificationTarget { Position = position };
        target.AddChild(new CollisionShape3D { Shape = new SphereShape3D { Radius = 0.8f } });
        target.AddChild(new Damageable { Name = "Health", MaximumHealth = 100.0f });
        return target;
    }

    private static AmmoHud CreateHud()
    {
        var hud = new AmmoHud { Name = "Hud" };
        hud.AddChild(new Label { Name = "Ammo" });
        return hud;
    }

    private void Expect(bool condition, string description)
    {
        if (condition)
            GD.Print($"PASS: {description}");
        else
            Fail(description);
    }

    private void Fail(string description)
    {
        _failures++;
        GD.PushError($"FAIL: {description}");
    }

    private partial class VerificationTarget : StaticBody3D, IDamageable
    {
        public Damageable Health => GetNode<Damageable>("Health");
        public bool IsDead => Health.IsDead;
        public void TakeDamage(float amount, Node source) => Health.TakeDamage(amount, source);
    }
}
