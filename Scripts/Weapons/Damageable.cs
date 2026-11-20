using Godot;

namespace BizzareWarfare.Weapons;

/// <summary>Reusable health component for future players, bots, and destructible targets.</summary>
public partial class Damageable : Node, IDamageable
{
    [Signal] public delegate void HealthChangedEventHandler(float currentHealth, float maximumHealth);
    [Signal] public delegate void DiedEventHandler(Node source);

    [Export] public float MaximumHealth { get; set; } = 100.0f;
    public float CurrentHealth { get; private set; }
    public bool IsDead { get; private set; }

    public override void _Ready()
    {
        ResetHealth();
    }

    public void TakeDamage(float amount, Node source)
    {
        if (IsDead || amount <= 0.0f || float.IsNaN(amount) || float.IsInfinity(amount))
            return;

        CurrentHealth = Mathf.Max(0.0f, CurrentHealth - amount);
        EmitSignal(SignalName.HealthChanged, CurrentHealth, MaximumHealth);
        if (CurrentHealth > 0.0f)
            return;

        IsDead = true;
        EmitSignal(SignalName.Died, source);
    }

    public void Heal(float amount)
    {
        if (IsDead || amount <= 0.0f || float.IsNaN(amount) || float.IsInfinity(amount))
            return;

        CurrentHealth = Mathf.Min(MaximumHealth, CurrentHealth + amount);
        EmitSignal(SignalName.HealthChanged, CurrentHealth, MaximumHealth);
    }

    public void ResetHealth()
    {
        MaximumHealth = Mathf.Max(1.0f, MaximumHealth);
        CurrentHealth = MaximumHealth;
        IsDead = false;
        EmitSignal(SignalName.HealthChanged, CurrentHealth, MaximumHealth);
    }
}
