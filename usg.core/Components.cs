using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Graphics;

namespace usg.core;

public struct Image2D
{
    public Texture2D Texture;
}
public struct Animation2D
{
    public AnimatedSprite Sprite;
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
