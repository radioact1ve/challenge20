using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace usg.core;

public struct Image2D
{
    public Texture2D Texture;
}

public struct Position
{
    public Vector2 Value;
}

public struct Velocity
{
    public Vector2 Value;
}

public sealed class InputState
{
    public KeyboardState Keyboard;
    public KeyboardState PreviousKeyboard;
    public GamePadState GamePad;

    public bool Pressed(Keys k) => Keyboard.IsKeyDown(k) && !PreviousKeyboard.IsKeyDown(k);
}
