using Godot;

namespace BizzareWarfare.Weapons;

/// <summary>Automatic hitscan rifle with camera-aligned targeting and lightweight visual effects.</summary>
public partial class Rifle : WeaponBase
{
    [Export] public float Range { get; set; } = 120.0f;
    [Export] public NodePath CameraPath { get; set; } = new NodePath();

    private Camera3D _camera = null!;
    private OmniLight3D _muzzleLight = null!;
    private MeshInstance3D _muzzleMesh = null!;
    private double _nextShotTime;
    private bool _requireFireRelease;

    public override void _Ready()
    {
        base._Ready();
        _camera = GetNode<Camera3D>(CameraPath);
        _muzzleLight = GetNode<OmniLight3D>("Muzzle/MuzzleLight");
        _muzzleMesh = GetNode<MeshInstance3D>("Muzzle/MuzzleMesh");
        SetMuzzleVisible(false);
    }

    public override void _Process(double delta)
    {
        if (!Input.IsActionPressed("fire"))
            _requireFireRelease = false;
        else if (!_requireFireRelease)
            TryFire();
        if (Input.IsActionJustPressed("reload"))
            TryReload();
    }

    public override void SetOperational(bool operational)
    {
        base.SetOperational(operational);
        if (!operational)
            _requireFireRelease = true;
    }

    public override void TryFire()
    {
        if (!CanFire || Time.GetTicksMsec() < _nextShotTime)
            return;

        _nextShotTime = Time.GetTicksMsec() + (ulong)(1000.0f / FireRate);
        ConsumeRound();
        EmitSignal(SignalName.ShotFired);
        FlashMuzzle();
        FireHitscan();
    }

    private void FireHitscan()
    {
        Vector3 origin = _camera.GlobalPosition;
        Vector3 end = origin + (-_camera.GlobalTransform.Basis.Z * Range);
        var query = PhysicsRayQueryParameters3D.Create(origin, end);
        Node? player = GetOwner();
        if (player is CollisionObject3D playerBody)
            query.Exclude = new Godot.Collections.Array<Rid> { playerBody.GetRid() };

        var hit = GetWorld3D().DirectSpaceState.IntersectRay(query);
        if (hit.Count == 0)
            return;

        Node? hitNode = hit["collider"].AsGodotObject() as Node;
        for (Node? candidate = hitNode; candidate != null; candidate = candidate.GetParent())
        {
            if (candidate is IDamageable damageable)
            {
                damageable.TakeDamage(Damage, this);
                break;
            }
        }

        if (hit.TryGetValue("position", out Variant position))
            SpawnImpact(position.AsVector3(), hit["normal"].AsVector3());
    }

    private async void FlashMuzzle()
    {
        SetMuzzleVisible(true);
        await ToSignal(GetTree().CreateTimer(0.045), SceneTreeTimer.SignalName.Timeout);
        if (IsInstanceValid(this))
            SetMuzzleVisible(false);
    }

    private void SetMuzzleVisible(bool visible)
    {
        _muzzleLight.Visible = visible;
        _muzzleMesh.Visible = visible;
    }

    private async void SpawnImpact(Vector3 position, Vector3 normal)
    {
        var impact = new MeshInstance3D
        {
            Mesh = new SphereMesh { Radius = 0.035f, Height = 0.07f },
            MaterialOverride = new StandardMaterial3D
            {
                AlbedoColor = new Color(1.0f, 0.58f, 0.12f),
                EmissionEnabled = true,
                Emission = new Color(1.0f, 0.18f, 0.01f),
                EmissionEnergyMultiplier = 3.0f
            },
            Position = position + normal * 0.025f
        };
        GetTree().CurrentScene.AddChild(impact);
        await ToSignal(GetTree().CreateTimer(0.12), SceneTreeTimer.SignalName.Timeout);
        if (IsInstanceValid(impact))
            impact.QueueFree();
    }
}
