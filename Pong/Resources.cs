using Microsoft.Xna.Framework.Input;

namespace Pong;

public sealed class InputState
{
    public KeyboardState Keyboard;
    public KeyboardState PreviousKeyboard;
    public GamePadState GamePad;

    public bool Pressed(Keys k) => Keyboard.IsKeyDown(k) && !PreviousKeyboard.IsKeyDown(k);
}

public sealed class Screen { public int Width, Height; }

public sealed class Score { public int Left, Right; }
