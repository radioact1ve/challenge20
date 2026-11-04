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

public class GravitySystem : ISystem
{
    public void Update(SystemContext ctx, GameTime gameTime)
    {
        var dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var g = ctx.Resource<Gravity>();
        var scales = ctx.Store<GravityScale>();
        var velocities = ctx.Store<Velocity>();

        foreach (int e in scales.Entities)
        {
            if (!velocities.Has(e)) continue;

            var scale = scales.Get(e);
            ref var velocity = ref velocities.Get(e);
            velocity.Value += g.Value * scale.Value * dt;
        }
    }
}

public class MovementSystem : ISystem
{
    public void Update(SystemContext ctx, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var positions = ctx.Store<Position>();
        var velocities = ctx.Store<Velocity>();
        var screen = ctx.Resource<Screen>();

        foreach (int e in positions.Entities)
        {
            if (!velocities.Has(e))
                continue;

            ref var position = ref positions.Get(e);
            ref var velocity = ref velocities.Get(e);

            position.Value += velocity.Value * dt;

            if (position.Value.Y >= screen.Height - 64) // ugly hack to deal with bird's image height.
            {   // completely stop the bird
                position.Value.Y = screen.Height - 64;
                velocity.Value.Y = 0;
            }
        }
    }
}