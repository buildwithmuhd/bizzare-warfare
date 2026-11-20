using Godot;

namespace BizzareWarfare.Player;

/// <summary>Reusable player spawn marker. Multiple markers can be placed in an arena.</summary>
public partial class SpawnPoint : Marker3D
{
    public override void _Ready() => AddToGroup("player_spawn_points");
}
