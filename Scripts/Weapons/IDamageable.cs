using Godot;

namespace BizzareWarfare.Weapons;

/// <summary>Implemented by anything that can receive combat damage.</summary>
public interface IDamageable
{
    bool IsDead { get; }
    void TakeDamage(float amount, Node source);
}
