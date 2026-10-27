using Microsoft.Xna.Framework;
using System;
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

public class PaddleMovementSystem : ISystem
{
    public void Update(SystemContext ctx, GameTime gameTime)
    {
        var input = ctx.Resource<InputState>();
        var screen = ctx.Resource<Screen>();

        var players = ctx.Store<PlayerControlled>();
        var velocities = ctx.Store<Velocity>();
        var positions = ctx.Store<Position>();

        foreach (int e in players.Entities)
        {
            if (!velocities.Has(e)) continue;
            ref var p = ref players.Get(e);

            float dir = 0;
            if (input.Keyboard.IsKeyDown(p.Up)) dir -= 1;
            if (input.Keyboard.IsKeyDown(p.Down)) dir += 1;

            var size = PongRulesSystem.Size(ctx, ctx.Handle(e));

            positions.Get(e).Value.Y = MathHelper.Clamp(positions.Get(e).Value.Y, 0, screen.Height - size.Y);

            velocities.Get(e).Value = new Vector2(0, dir * p.Speed);
        }
    }
}

public class PongRulesSystem : ISystem
{
    private readonly Entity ball, left, right;

    public PongRulesSystem(Entity ball, Entity left, Entity right)
    {
        this.ball = ball;
        this.left = left;
        this.right = right;
    }

    public void Update(SystemContext ctx, GameTime gameTime)
    {
        var screen = ctx.Resource<Screen>();
        var score = ctx.Resource<Score>();

        ref var pos = ref ctx.Get<Position>(ball).Value;
        ref var vel = ref ctx.Get<Velocity>(ball).Value;
        Vector2 size = Size(ctx, ball);

        // Walls: clamp, then force the velocity away from the wall.
        if (pos.Y < 0)
        {
            pos.Y = 0;
            vel.Y = MathF.Abs(vel.Y);
        }
        else if (pos.Y + size.Y > screen.Height)
        {
            pos.Y = screen.Height - size.Y;
            vel.Y = -MathF.Abs(vel.Y);
        }

        // Paddles: dir is where the ball should go after the hit.
        BounceOffPaddle(ctx, left, ref pos, ref vel, size, dir: +1);
        BounceOffPaddle(ctx, right, ref pos, ref vel, size, dir: -1);

        // Goals.
        if (pos.X + size.X < 0) { score.Right++; Serve(ref pos, ref vel, size, screen, dir: -1); }
        else if (pos.X > screen.Width) { score.Left++; Serve(ref pos, ref vel, size, screen, dir: +1); }
    }

    private static void BounceOffPaddle(SystemContext ctx, Entity paddle,
        ref Vector2 pos, ref Vector2 vel, Vector2 size, int dir)
    {
        // only when moving toward this paddle
        if (MathF.Sign(vel.X) != -dir) return;

        Vector2 p = ctx.Get<Position>(paddle).Value;
        Vector2 ps = Size(ctx, paddle);

        bool overlap = pos.X < p.X + ps.X && pos.X + size.X > p.X
                    && pos.Y < p.Y + ps.Y && pos.Y + size.Y > p.Y;
        if (!overlap) return;

        pos.X = dir > 0 ? p.X + ps.X : p.X - size.X;     // push out
        vel.X = -vel.X;
        vel.Y += ((pos.Y + size.Y / 2) - (p.Y + ps.Y / 2)) * 4f;   // english
    }

    private static void Serve(ref Vector2 pos, ref Vector2 vel, Vector2 size, Screen screen, int dir)
    {
        pos = new Vector2(screen.Width, screen.Height) / 2 - size / 2;
        vel = new Vector2(300 * dir, 300);
    }

    // DrawSystem draws at the texture's native size, so the texture size is the hitbox.
    public static Vector2 Size(SystemContext ctx, Entity e)
    {
        var t = ctx.Get<Image2D>(e).Texture;
        return new Vector2(t.Width, t.Height);
    }
}