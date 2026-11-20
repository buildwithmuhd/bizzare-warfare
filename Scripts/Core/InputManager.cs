using Godot;

namespace BizzareWarfare.Core;

/// <summary>Centralizes controller mappings, analog processing, settings, and active-input tracking.</summary>
public partial class InputManager : Node
{
    public enum InputSource { KeyboardMouse, Controller }

    [Signal] public delegate void ActiveInputChangedEventHandler(InputSource source);

    [Export(PropertyHint.Range, "0.0,0.9,0.01")] public float MoveDeadzone { get; private set; } = 0.18f;
    [Export(PropertyHint.Range, "0.0,0.9,0.01")] public float LookDeadzone { get; private set; } = 0.15f;
    [Export(PropertyHint.Range, "0.1,12.0,0.1")] public float HorizontalLookSensitivity { get; private set; } = 3.2f;
    [Export(PropertyHint.Range, "0.1,12.0,0.1")] public float VerticalLookSensitivity { get; private set; } = 2.7f;

    public InputSource ActiveSource { get; private set; } = InputSource.KeyboardMouse;

    public override void _Ready()
    {
        EnsureMappings();
        LoadSettings();
    }

    public override void _Input(InputEvent inputEvent)
    {
        RegisterInput(inputEvent);
    }

    public Vector2 GetMoveVector() => Input.GetVector("move_left", "move_right", "move_forward", "move_back", MoveDeadzone);

    public Vector2 GetControllerLookVector()
    {
        var raw = new Vector2(Input.GetJoyAxis(0, JoyAxis.RightX), Input.GetJoyAxis(0, JoyAxis.RightY));
        var filtered = ApplyRadialDeadzone(raw, LookDeadzone);
        if (filtered != Vector2.Zero)
            SetActiveSource(InputSource.Controller);
        return filtered;
    }

    public void RegisterInput(InputEvent inputEvent)
    {
        if (inputEvent is InputEventJoypadMotion motion && Mathf.Abs(motion.AxisValue) > 0.05f)
            SetActiveSource(InputSource.Controller);
        else if (inputEvent is InputEventJoypadButton button && button.Pressed)
            SetActiveSource(InputSource.Controller);
        else if (inputEvent is InputEventKey { Pressed: true } || inputEvent is InputEventMouseMotion || inputEvent is InputEventMouseButton { Pressed: true })
            SetActiveSource(InputSource.KeyboardMouse);
    }

    public void SetControllerSettings(float moveDeadzone, float lookDeadzone, float horizontalSensitivity, float verticalSensitivity)
    {
        MoveDeadzone = Mathf.Clamp(moveDeadzone, 0.0f, 0.9f);
        LookDeadzone = Mathf.Clamp(lookDeadzone, 0.0f, 0.9f);
        HorizontalLookSensitivity = Mathf.Max(horizontalSensitivity, 0.1f);
        VerticalLookSensitivity = Mathf.Max(verticalSensitivity, 0.1f);
        SaveSettings();
    }

    public static Vector2 ApplyRadialDeadzone(Vector2 input, float deadzone)
    {
        float magnitude = input.Length();
        if (magnitude <= deadzone)
            return Vector2.Zero;

        float scaledMagnitude = Mathf.Clamp((magnitude - deadzone) / (1.0f - deadzone), 0.0f, 1.0f);
        return input / magnitude * scaledMagnitude;
    }

    public static void EnsureMappings()
    {
        AddKeyAction("move_forward", Key.W);
        AddKeyAction("move_back", Key.S);
        AddKeyAction("move_left", Key.A);
        AddKeyAction("move_right", Key.D);
        AddKeyAction("jump", Key.Space);
        AddKeyAction("sprint", Key.Shift);
        AddKeyAction("reload", Key.R);
        AddKeyAction("pause", Key.Escape);
        AddKeyAction("respawn", Key.Enter);
        AddMouseAction("fire", MouseButton.Left);

        AddJoyAxisAction("move_left", JoyAxis.LeftX, -1.0f);
        AddJoyAxisAction("move_right", JoyAxis.LeftX, 1.0f);
        AddJoyAxisAction("move_forward", JoyAxis.LeftY, -1.0f);
        AddJoyAxisAction("move_back", JoyAxis.LeftY, 1.0f);
        AddJoyAxisAction("look_left", JoyAxis.RightX, -1.0f);
        AddJoyAxisAction("look_right", JoyAxis.RightX, 1.0f);
        AddJoyAxisAction("look_up", JoyAxis.RightY, -1.0f);
        AddJoyAxisAction("look_down", JoyAxis.RightY, 1.0f);
        AddJoyAxisAction("fire", JoyAxis.TriggerRight, 1.0f);
        AddJoyButtonAction("jump", JoyButton.A);
        AddJoyButtonAction("sprint", JoyButton.LeftStick);
        AddJoyButtonAction("reload", JoyButton.X);
        AddJoyButtonAction("pause", JoyButton.Start);
        AddJoyButtonAction("respawn", JoyButton.A);
    }

    private void SetActiveSource(InputSource source)
    {
        if (ActiveSource == source)
            return;

        ActiveSource = source;
        EmitSignal(SignalName.ActiveInputChanged, (int)source);
    }

    private void LoadSettings()
    {
        var config = new ConfigFile();
        if (config.Load("user://input_settings.cfg") != Error.Ok)
            return;

        MoveDeadzone = (float)config.GetValue("controller", "move_deadzone", MoveDeadzone);
        LookDeadzone = (float)config.GetValue("controller", "look_deadzone", LookDeadzone);
        HorizontalLookSensitivity = (float)config.GetValue("controller", "horizontal_sensitivity", HorizontalLookSensitivity);
        VerticalLookSensitivity = (float)config.GetValue("controller", "vertical_sensitivity", VerticalLookSensitivity);
    }

    private void SaveSettings()
    {
        var config = new ConfigFile();
        config.SetValue("controller", "move_deadzone", MoveDeadzone);
        config.SetValue("controller", "look_deadzone", LookDeadzone);
        config.SetValue("controller", "horizontal_sensitivity", HorizontalLookSensitivity);
        config.SetValue("controller", "vertical_sensitivity", VerticalLookSensitivity);
        config.Save("user://input_settings.cfg");
    }

    private static void AddKeyAction(string action, Key key)
    {
        EnsureAction(action);
        if (!HasEvent<InputEventKey>(action, input => input.PhysicalKeycode == key))
            InputMap.ActionAddEvent(action, new InputEventKey { PhysicalKeycode = key });
    }

    private static void AddMouseAction(string action, MouseButton button)
    {
        EnsureAction(action);
        if (!HasEvent<InputEventMouseButton>(action, input => input.ButtonIndex == button))
            InputMap.ActionAddEvent(action, new InputEventMouseButton { ButtonIndex = button });
    }

    private static void AddJoyButtonAction(string action, JoyButton button)
    {
        EnsureAction(action);
        if (!HasEvent<InputEventJoypadButton>(action, input => input.ButtonIndex == button))
            InputMap.ActionAddEvent(action, new InputEventJoypadButton { ButtonIndex = button });
    }

    private static void AddJoyAxisAction(string action, JoyAxis axis, float axisValue)
    {
        EnsureAction(action);
        if (!HasEvent<InputEventJoypadMotion>(action, input => input.Axis == axis && Mathf.IsEqualApprox(input.AxisValue, axisValue)))
            InputMap.ActionAddEvent(action, new InputEventJoypadMotion { Axis = axis, AxisValue = axisValue });
    }

    private static void EnsureAction(string action)
    {
        if (!InputMap.HasAction(action))
            InputMap.AddAction(action);
    }

    private static bool HasEvent<T>(string action, System.Func<T, bool> match) where T : InputEvent
    {
        foreach (InputEvent inputEvent in InputMap.ActionGetEvents(action))
        {
            if (inputEvent is T typed && match(typed))
                return true;
        }
        return false;
    }
}
