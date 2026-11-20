using Godot;
using BizzareWarfare.Player;

namespace BizzareWarfare.UI;

/// <summary>Minimal signal-driven player-health and death presentation.</summary>
public partial class PlayerLifecycleHud : CanvasLayer
{
    private Label _healthLabel = null!;
    private Control _deathPanel = null!;

    public override void _Ready()
    {
        _healthLabel = GetNode<Label>("Health");
        _deathPanel = GetNode<Control>("DeathPanel");
        _deathPanel.Visible = false;

        FpsPlayer? player = GetTree().GetFirstNodeInGroup("player") as FpsPlayer;
        if (player == null)
        {
            _healthLabel.Text = "HEALTH -- / --";
            return;
        }

        player.GetNode<BizzareWarfare.Weapons.Damageable>("Damageable").HealthChanged += UpdateHealth;
        player.PlayerDied += ShowDeath;
        player.PlayerRespawned += HideDeath;
        UpdateHealth(player.CurrentHealth, player.MaximumHealth);
    }

    private void UpdateHealth(float currentHealth, float maximumHealth) => _healthLabel.Text = $"HEALTH {Mathf.CeilToInt(currentHealth):000} / {Mathf.CeilToInt(maximumHealth):000}";

    private void ShowDeath(Node source) => _deathPanel.Visible = true;

    private void HideDeath()
    {
        _deathPanel.Visible = false;
    }
}
