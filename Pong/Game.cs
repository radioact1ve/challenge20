using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using usg.core;
using static Pong.Game;

namespace Pong;

public class Game : Core
{
    private World world;
    private Entity ball;
    private Entity paddle1;
    private Entity paddle2;
    public Game() : base("Pong", 1920, 1080, false)
    {
        world = new World();
        world.AddDrawSystem(new DrawSystem());

        ball = world.EntityCreate();
        world.AddComponent(ball, new Position { Value = new Vector2(960, 540) });

        paddle1 = world.EntityCreate();
        world.AddComponent(paddle1, new Position { Value = new Vector2(100, 540) });

        paddle2 = world.EntityCreate();
        world.AddComponent(paddle2, new Position { Value = new Vector2(1820, 540) });
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        world.AddComponent(ball, new Image2DComponent { Texture = Content.Load<Texture2D>("pong/Ball") });
        world.AddComponent(paddle1, new Image2DComponent { Texture = Content.Load<Texture2D>("pong/Paddle_1") });
        world.AddComponent(paddle2, new Image2DComponent { Texture = Content.Load<Texture2D>("pong/Paddle_2") });
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        world.Draw(gameTime);
        SpriteBatch.End();
    }

    public struct Image2DComponent
    {
        public Texture2D Texture;
    }

    public struct Position
    {
        public Vector2 Value;
    }
}

public class DrawSystem : IDraw
{
    public void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
    {
        foreach (int e in world.Entities<Image2DComponent>())
        {
            if (!world.HasComponent<Position>(e))
                continue;

            ref var transform = ref world.GetComponent<Position>(e);
            ref var sprite = ref world.GetComponent<Image2DComponent>(e);

            spriteBatch.Draw(sprite.Texture, transform.Value, Color.White);
        }
    }
}
