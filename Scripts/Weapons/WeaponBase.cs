using Godot;

namespace BizzareWarfare.Weapons;

/// <summary>Common ammunition and firing contract used by all first-person weapons.</summary>
public abstract partial class WeaponBase : Node3D
{
    [Signal] public delegate void AmmoChangedEventHandler(int magazineAmmo, int reserveAmmo, bool isReloading);
    [Signal] public delegate void ShotFiredEventHandler();
    [Signal] public delegate void ReloadStartedEventHandler();
    [Signal] public delegate void ReloadFinishedEventHandler();

    [Export] public string WeaponName { get; set; } = "Weapon";
    [Export] public float Damage { get; set; } = 25.0f;
    [Export(PropertyHint.Range, "1,120,1")] public float FireRate { get; set; } = 10.0f;
    [Export] public int MagazineCapacity { get; set; } = 30;
    [Export] public int StartingReserveAmmo { get; set; } = 90;
    [Export] public float ReloadDuration { get; set; } = 1.6f;

    public int MagazineAmmo { get; protected set; }
    public int ReserveAmmo { get; protected set; }
    public bool IsReloading { get; protected set; }
    public bool IsOperational { get; private set; } = true;
    public bool CanFire => IsOperational && !IsReloading && MagazineAmmo > 0;
    private int _reloadVersion;

    public override void _Ready()
    {
        MagazineAmmo = MagazineCapacity;
        ReserveAmmo = StartingReserveAmmo;
        EmitAmmoChanged();
    }

    public abstract void TryFire();

    public async void TryReload()
    {
        if (!IsOperational || IsReloading || MagazineAmmo >= MagazineCapacity || ReserveAmmo <= 0)
            return;

        int reloadVersion = _reloadVersion;
        IsReloading = true;
        EmitSignal(SignalName.ReloadStarted);
        EmitAmmoChanged();
        await ToSignal(GetTree().CreateTimer(ReloadDuration), SceneTreeTimer.SignalName.Timeout);

        if (!IsOperational || reloadVersion != _reloadVersion)
            return;

        int needed = MagazineCapacity - MagazineAmmo;
        int loaded = Mathf.Min(needed, ReserveAmmo);
        MagazineAmmo += loaded;
        ReserveAmmo -= loaded;
        IsReloading = false;
        EmitAmmoChanged();
        EmitSignal(SignalName.ReloadFinished);
    }

    protected void ConsumeRound()
    {
        MagazineAmmo--;
        EmitAmmoChanged();
    }

    protected void EmitAmmoChanged() => EmitSignal(SignalName.AmmoChanged, MagazineAmmo, ReserveAmmo, IsReloading);

    /// <summary>Enables or disables a weapon without leaving an in-flight reload active.</summary>
    public virtual void SetOperational(bool operational)
    {
        IsOperational = operational;
        if (!operational)
            CancelReload();
    }

    /// <summary>Prototype respawn policy: restore configured magazine and reserve ammunition.</summary>
    public void ResetForRespawn()
    {
        CancelReload();
        MagazineAmmo = MagazineCapacity;
        ReserveAmmo = StartingReserveAmmo;
        IsOperational = true;
        EmitAmmoChanged();
    }

    private void CancelReload()
    {
        _reloadVersion++;
        if (!IsReloading)
            return;

        IsReloading = false;
        EmitAmmoChanged();
    }
}
