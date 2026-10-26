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
        world.AddSystem(new PaddleMovementSystem());
        world.AddSystem(new MovementSystem());
        world.AddResource(new Score());
        world.AddResource(new InputState());


        ball = world.EntityCreate();
        world.AddComponent(ball, new Position { Value = new Vector2(960, 540) });
        world.AddComponent(ball, new Velocity { Value = new Vector2(300, 300) });

        paddle1 = world.EntityCreate();
        world.AddComponent(paddle1, new Position { Value = new Vector2(100, 540) });
        world.AddComponent(paddle1, new Velocity { Value = new Vector2(0, 0) });
        world.AddComponent(paddle1, new PlayerControlled { Up = Keys.W, Down = Keys.S, Speed = 300 });

        paddle2 = world.EntityCreate();
        world.AddComponent(paddle2, new Position { Value = new Vector2(1820, 540) });
        world.AddComponent(paddle2, new Velocity { Value = new Vector2(0, 0) });
        world.AddComponent(paddle2, new PlayerControlled { Up = Keys.Up, Down = Keys.Down, Speed = 300 });

        world.AddSystem(new PongRulesSystem(ball, paddle1, paddle2));
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        world.AddComponent(ball, new Image2D { Texture = Content.Load<Texture2D>("pong/Ball") });
        world.AddComponent(paddle1, new Image2D { Texture = Content.Load<Texture2D>("pong/Paddle_1") });
        world.AddComponent(paddle2, new Image2D { Texture = Content.Load<Texture2D>("pong/Paddle_2") });

        world.AddResource(new Screen
        {
            Width = GraphicsDevice.Viewport.Width,
            Height = GraphicsDevice.Viewport.Height
        });
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        var input = world.GetResource<InputState>();
        input.PreviousKeyboard = input.Keyboard;
        input.Keyboard = Keyboard.GetState();
        input.GamePad = GamePad.GetState(PlayerIndex.One);

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