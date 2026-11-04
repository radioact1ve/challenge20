using Microsoft.Xna.Framework;

namespace flappy;

// random number I found online, trying. Positive Y is down in MonoGame, so this is a downward force.
public sealed class Gravity { public Vector2 Value = new(0, 980f); }

public sealed class Screen { public int Width, Height; }
