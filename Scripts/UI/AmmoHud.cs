using Godot;
using BizzareWarfare.Weapons;

namespace BizzareWarfare.UI;

/// <summary>Signal-driven ammunition readout, independent of the rifle's concrete implementation.</summary>
public partial class AmmoHud : CanvasLayer
{
    private Label _ammoLabel = null!;

    public override void _Ready()
    {
        _ammoLabel = GetNode<Label>("Ammo");
        WeaponBase? weapon = GetTree().GetFirstNodeInGroup("player_weapon") as WeaponBase;
        if (weapon == null)
        {
            _ammoLabel.Text = "RIFLE -- / --";
            return;
        }
        weapon.AmmoChanged += UpdateAmmo;
        UpdateAmmo(weapon.MagazineAmmo, weapon.ReserveAmmo, weapon.IsReloading);
    }

    private void UpdateAmmo(int magazine, int reserve, bool reloading) => _ammoLabel.Text = reloading ? "RIFLE RELOADING" : $"RIFLE {magazine:00} / {reserve:000}";
}
