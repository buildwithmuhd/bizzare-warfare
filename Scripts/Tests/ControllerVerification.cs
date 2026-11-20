using Godot;
using BizzareWarfare.Core;

namespace BizzareWarfare.Tests;

/// <summary>Headless coverage for Milestone 4's controller mappings and analog input processing.</summary>
public partial class ControllerVerification : Node
{
    private int _failures;

    public override async void _Ready()
    {
        try
        {
            var input = new InputManager();
            AddChild(input);
            await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

            VerifyRequiredMappings();
            VerifyDeadzoneMath();
            VerifyActiveInputSwitching(input);
            await VerifyAnalogActions(input);
        }
        catch (System.Exception exception)
        {
            Fail($"Unhandled test exception: {exception.Message}");
        }

        GD.Print($"CONTROLLER_VERIFICATION: {(_failures == 0 ? "PASS" : "FAIL")} ({_failures} failures)");
        GetTree().Quit(_failures == 0 ? 0 : 1);
    }

    private void VerifyRequiredMappings()
    {
        Expect(HasJoyAxis("move_left", JoyAxis.LeftX, -1.0f) && HasJoyAxis("move_right", JoyAxis.LeftX, 1.0f), "left stick horizontal movement mappings exist");
        Expect(HasJoyAxis("move_forward", JoyAxis.LeftY, -1.0f) && HasJoyAxis("move_back", JoyAxis.LeftY, 1.0f), "left stick vertical movement mappings exist");
        Expect(HasJoyAxis("look_left", JoyAxis.RightX, -1.0f) && HasJoyAxis("look_right", JoyAxis.RightX, 1.0f), "right stick horizontal look mappings exist");
        Expect(HasJoyAxis("look_up", JoyAxis.RightY, -1.0f) && HasJoyAxis("look_down", JoyAxis.RightY, 1.0f), "right stick vertical look mappings exist");
        Expect(HasJoyAxis("fire", JoyAxis.TriggerRight, 1.0f), "right trigger fire mapping exists");
        Expect(HasJoyButton("jump", JoyButton.A), "A/Cross jump mapping exists");
        Expect(HasJoyButton("sprint", JoyButton.LeftStick), "left-stick sprint mapping exists");
        Expect(HasJoyButton("reload", JoyButton.X), "X/Square reload mapping exists");
        Expect(HasJoyButton("pause", JoyButton.Start), "Start/Options pause mapping exists");
    }

    private void VerifyDeadzoneMath()
    {
        Expect(InputManager.ApplyRadialDeadzone(new Vector2(0.1f, 0.0f), 0.18f) == Vector2.Zero, "stick values inside the deadzone are ignored");

        Vector2 partial = InputManager.ApplyRadialDeadzone(new Vector2(0.59f, 0.0f), 0.18f);
        Expect(partial.X > 0.0f && partial.X < 1.0f && Mathf.IsZeroApprox(partial.Y), "partial stick deflection stays analog after deadzone scaling");

        Vector2 diagonal = InputManager.ApplyRadialDeadzone(new Vector2(1.0f, 1.0f).Normalized(), 0.18f);
        Expect(Mathf.IsEqualApprox(diagonal.Length(), 1.0f), "full diagonal stick deflection is normalized to maximum strength");
    }

    private void VerifyActiveInputSwitching(InputManager input)
    {
        input.RegisterInput(new InputEventJoypadMotion { Axis = JoyAxis.RightX, AxisValue = 0.5f });
        Expect(input.ActiveSource == InputManager.InputSource.Controller, "joypad motion selects controller input mode");

        input.RegisterInput(new InputEventKey { Pressed = true, Keycode = Key.W });
        Expect(input.ActiveSource == InputManager.InputSource.KeyboardMouse, "keyboard input selects keyboard/mouse mode");
    }

    private async System.Threading.Tasks.Task VerifyAnalogActions(InputManager input)
    {
        Input.ActionPress("move_right", 0.5f);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Vector2 partialMove = input.GetMoveVector();
        Expect(partialMove.X > 0.0f && partialMove.X < 1.0f, "movement processing preserves partial analog speed");
        Input.ActionRelease("move_right");

        Input.ActionPress("move_right", 1.0f);
        Input.ActionPress("move_back", 1.0f);
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Vector2 diagonalMove = input.GetMoveVector();
        Expect(diagonalMove.Length() <= 1.001f && diagonalMove.Length() > 0.99f, "diagonal movement is normalized to prevent speed gain");
        Input.ActionRelease("move_right");
        Input.ActionRelease("move_back");

        Input.ParseInputEvent(new InputEventJoypadMotion { Device = 0, Axis = JoyAxis.TriggerRight, AxisValue = 1.0f });
        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
        Expect(Input.IsActionPressed("fire"), "right-trigger motion activates the shared fire action");
        Input.ParseInputEvent(new InputEventJoypadMotion { Device = 0, Axis = JoyAxis.TriggerRight, AxisValue = 0.0f });
    }

    private static bool HasJoyAxis(string action, JoyAxis axis, float axisValue)
    {
        foreach (InputEvent inputEvent in InputMap.ActionGetEvents(action))
        {
            if (inputEvent is InputEventJoypadMotion motion && motion.Axis == axis && Mathf.IsEqualApprox(motion.AxisValue, axisValue))
                return true;
        }

        return false;
    }

    private static bool HasJoyButton(string action, JoyButton button)
    {
        foreach (InputEvent inputEvent in InputMap.ActionGetEvents(action))
        {
            if (inputEvent is InputEventJoypadButton joyButton && joyButton.ButtonIndex == button)
                return true;
        }

        return false;
    }

    private void Expect(bool condition, string description)
    {
        if (condition)
            GD.Print($"PASS: {description}");
        else
            Fail(description);
    }

    private void Fail(string description)
    {
        _failures++;
        GD.PushError($"FAIL: {description}");
    }
}
