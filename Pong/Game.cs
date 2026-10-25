using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using usg.core;

namespace Pong;

public class Game : Core
{
    private readonly World world;
    private Entity ball;
    private Entity paddle1;
    private Entity paddle2;
    public Game() : base("Pong", 1920, 1080, false)
    {
        world = new World();
        Defaults.WithDefaultSystems(world);
        world.AddSystem(new MovementSystem());

        ball = world.EntityCreate();
        world.AddComponent(ball, new Position { Value = new Vector2(960, 540) });
        world.AddComponent(ball, new Velocity { Value = new Vector2(300, 300) });

        paddle1 = world.EntityCreate();
        world.AddComponent(paddle1, new Position { Value = new Vector2(100, 540) });

        paddle2 = world.EntityCreate();
        world.AddComponent(paddle2, new Position { Value = new Vector2(1820, 540) });
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        world.AddComponent(ball, new Image2D { Texture = Content.Load<Texture2D>("pong/Ball") });
        world.AddComponent(paddle1, new Image2D { Texture = Content.Load<Texture2D>("pong/Paddle_1") });
        world.AddComponent(paddle2, new Image2D { Texture = Content.Load<Texture2D>("pong/Paddle_2") });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        base.Update(gameTime);
        world.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);
        world.Draw(gameTime, SpriteBatch);
        SpriteBatch.End();
    }
}