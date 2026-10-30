using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace flappy;

public struct Bird;

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