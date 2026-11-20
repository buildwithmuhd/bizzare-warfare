using Godot;

namespace BizzareWarfare.Weapons;

/// <summary>Temporary damage receiver proving the weapon-to-health contract before bot AI exists.</summary>
public partial class TestTarget : StaticBody3D, IDamageable
{
    private Damageable _health = null!;
    private Label3D _label = null!;

    public override void _Ready()
    {
        _health = GetNode<Damageable>("Damageable");
        _label = GetNode<Label3D>("HealthLabel");
        _health.HealthChanged += UpdateLabel;
        _health.Died += HandleDeath;
        UpdateLabel(_health.CurrentHealth, _health.MaximumHealth);
    }

    private void UpdateLabel(float current, float maximum) => _label.Text = current > 0 ? $"TARGET {Mathf.CeilToInt(current)}" : "TARGET DOWN";

    private void HandleDeath(Node source)
    {
        GetNode<MeshInstance3D>("Mesh").MaterialOverride = new StandardMaterial3D { AlbedoColor = new Color(0.22f, 0.03f, 0.03f) };
    }

    public bool IsDead => _health.IsDead;

    public void TakeDamage(float amount, Node source) => _health.TakeDamage(amount, source);
}
