using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Pong;

public struct Ball;

public struct PlayerControlled
{
    public Keys Up, Down;
    public float Speed;
}

public struct AIControlled
{
    public float Speed;
}

public struct Font
{
    public SpriteFont Value;
}