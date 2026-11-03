using Microsoft.Xna.Framework;
using usg.core;

namespace flappy;

public class AnimationSystem : ISystem
{
    public void Update(SystemContext ctx, GameTime gameTime)
    {
        var animations = ctx.Store<Animation2D>();

        foreach (int e in animations.Entities)
        {
            animations.Get(e).Sprite.Update(gameTime);
        }
    }
}