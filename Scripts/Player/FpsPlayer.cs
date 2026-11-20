using Godot;
using BizzareWarfare.Core;
using BizzareWarfare.Weapons;

namespace BizzareWarfare.Player;

/// <summary>Minimal, responsive first-person controller for the prototype arena.</summary>
public partial class FpsPlayer : CharacterBody3D, IDamageable
{
    [Signal] public delegate void PlayerDiedEventHandler(Node source);
    [Signal] public delegate void PlayerRespawnedEventHandler();

    [Export] public float WalkSpeed { get; set; } = 6.0f;
    [Export] public float SprintSpeed { get; set; } = 9.0f;
    [Export] public float JumpVelocity { get; set; } = 7.5f;
    [Export] public float Gravity { get; set; } = 24.0f;
    [Export] public float MouseSensitivity { get; set; } = 0.0025f;
    [Export] public float RespawnDelay { get; set; } = 3.0f;
    [Export] public bool AutoRespawn { get; set; } = true;

    private Camera3D _camera = null!;
    private InputManager _input = null!;
    private Damageable _health = null!;
    private WeaponBase _weapon = null!;
    private float _pitch;
    private bool _respawnPending;
    private bool _respawnAvailable;

    public float CurrentHealth => _health.CurrentHealth;
    public float MaximumHealth => _health.MaximumHealth;
    public bool IsDead => _health.IsDead;

    public override void _Ready()
    {
        _camera = GetNode<Camera3D>("Camera3D");
        _input = GetNode<InputManager>("InputManager");
        _health = GetNode<Damageable>("Damageable");
        _weapon = GetNode<WeaponBase>("Camera3D/Rifle");
        _health.Died += HandleDeath;
        AddToGroup("player");
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent inputEvent)
    {
        if (IsDead)
            return;
        if (inputEvent is InputEventKey { Pressed: true, Keycode: Key.Escape })
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            return;
        }

        if (inputEvent is InputEventMouseButton { Pressed: true } && Input.MouseMode != Input.MouseModeEnum.Captured)
        {
            Input.MouseMode = Input.MouseModeEnum.Captured;
            return;
        }

        if (inputEvent is not InputEventMouseMotion mouseMotion || Input.MouseMode != Input.MouseModeEnum.Captured)
            return;

        ApplyLook(-mouseMotion.Relative.X * MouseSensitivity, -mouseMotion.Relative.Y * MouseSensitivity);
    }

    public override void _Process(double delta)
    {
        if (IsDead)
        {
            if (_respawnAvailable && Input.IsActionJustPressed("respawn"))
                Respawn();
            return;
        }

        Vector2 controllerLook = _input.GetControllerLookVector();
        if (controllerLook != Vector2.Zero)
            ApplyLook(-controllerLook.X * _input.HorizontalLookSensitivity * (float)delta, -controllerLook.Y * _input.VerticalLookSensitivity * (float)delta);

        if (Input.IsActionJustPressed("pause"))
            Input.MouseMode = Input.MouseModeEnum.Visible;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsDead)
        {
            Velocity = Vector3.Zero;
            return;
        }

        var velocity = Velocity;
        if (!IsOnFloor())
            velocity.Y -= Gravity * (float)delta;

        if (Input.IsActionJustPressed("jump") && IsOnFloor())
            velocity.Y = JumpVelocity;

        Vector2 moveInput = _input.GetMoveVector();
        Vector3 localDirection = new(moveInput.X, 0.0f, moveInput.Y);
        Vector3 moveDirection = Transform.Basis * localDirection;
        float targetSpeed = Input.IsActionPressed("sprint") ? SprintSpeed : WalkSpeed;

        velocity.X = Mathf.MoveToward(velocity.X, moveDirection.X * targetSpeed, 30.0f * (float)delta);
        velocity.Z = Mathf.MoveToward(velocity.Z, moveDirection.Z * targetSpeed, 30.0f * (float)delta);
        Velocity = velocity;
        MoveAndSlide();
    }

    private void ApplyLook(float yawDelta, float pitchDelta)
    {
        RotateY(yawDelta);
        _pitch = Mathf.Clamp(_pitch + pitchDelta, -1.45f, 1.45f);
        _camera.Rotation = new Vector3(_pitch, 0.0f, 0.0f);
    }

    public void TakeDamage(float amount, Node source) => _health.TakeDamage(amount, source);

    private void HandleDeath(Node source)
    {
        if (_respawnPending)
            return;

        _respawnPending = true;
        _respawnAvailable = false;
        Velocity = Vector3.Zero;
        _weapon.SetOperational(false);
        Input.MouseMode = Input.MouseModeEnum.Visible;
        EmitSignal(SignalName.PlayerDied, source);
        BeginRespawnCountdown();
    }

    private async void BeginRespawnCountdown()
    {
        await ToSignal(GetTree().CreateTimer(Mathf.Max(0.0f, RespawnDelay)), SceneTreeTimer.SignalName.Timeout);
        if (!IsInstanceValid(this) || !IsDead)
            return;

        _respawnAvailable = true;
        if (AutoRespawn)
            Respawn();
    }

    public void Respawn()
    {
        if (!IsDead || !_respawnAvailable)
            return;

        Node3D? spawnPoint = GetTree().GetFirstNodeInGroup("player_spawn_points") as Node3D;
        if (spawnPoint != null)
            GlobalTransform = spawnPoint.GlobalTransform;

        Velocity = Vector3.Zero;
        _pitch = 0.0f;
        _camera.Rotation = Vector3.Zero;
        _health.ResetHealth();
        _weapon.ResetForRespawn();
        _respawnPending = false;
        _respawnAvailable = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        EmitSignal(SignalName.PlayerRespawned);
    }
}
