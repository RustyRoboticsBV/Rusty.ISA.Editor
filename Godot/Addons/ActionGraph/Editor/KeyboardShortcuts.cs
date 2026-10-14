using Godot;

namespace Rusty.ActionGraph.Editor;

/// <summary>
/// A utility for checking if a undo/redo keyboard shortcut has been pressed.
/// </summary>
internal static class KeyboardShortcuts
{
    /* Public properties. */
    public static bool PressedUndo
    {
        get
        {
            Update();
            return _PressedUndo;
        }
    }
    public static bool PressedRedo
    {
        get
        {
            Update();
            return _PressedRedo;
        }
    }

    /* Private properties. */
    private static double Timestamp { get; set; }
    private static bool PressAllowed { get; set; } = true;
    private static bool _PressedUndo { get; set; }
    private static bool _PressedRedo { get; set; }

    /* Private methods. */
    private static void Update()
    {
        // Update timestamp.
        if (Timestamp == Time.GetUnixTimeFromSystem())
            return;
        Timestamp = Time.GetUnixTimeFromSystem();

        // Update.
        if (_PressedUndo)
            _PressedUndo = false;
        if (_PressedRedo)
            _PressedRedo = false;
        if (Input.IsKeyPressed(Key.Ctrl))
        {
            if (PressAllowed)
            {
                if (Input.IsKeyPressed(Key.Z))
                {
                    _PressedUndo = true;
                    PressAllowed = false;
                }
                else if (Input.IsKeyPressed(Key.Y))
                {
                    _PressedRedo = true;
                    PressAllowed = false;
                }
                else
                    Clear();
            }
            else if (!Input.IsKeyPressed(Key.Z) && !Input.IsKeyPressed(Key.Y))
                PressAllowed = true;
        }
        else
            Clear();
    }

    private static void Clear()
    {
        _PressedUndo = false;
        _PressedRedo = false;
        PressAllowed = true;
    }
}
