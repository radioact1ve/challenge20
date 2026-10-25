using Microsoft.Xna.Framework;
using usg.core;

namespace Pong;

public class MovementSystem : ISystem
{
    public void Update(SystemContext ctx, GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        var positions = ctx.Store<Position>();
        var velocities = ctx.Store<Velocity>();

        foreach (int e in positions.Entities)
        {
            if (!velocities.Has(e))
                continue;

            ref var position = ref positions.Get(e);
            ref var velocity = ref velocities.Get(e);

            position.Value += velocity.Value * dt;
        }
    }
}
