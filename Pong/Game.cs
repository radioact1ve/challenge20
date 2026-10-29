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
    private Entity scoreboard;
    private Entity background;
    public Game() : base("Pong", 1280, 720, false)
    {
        world = new World();

        world.AddDrawSystem(new DrawSystem());
        world.AddDrawSystem(new DrawTextSystem());
        world.AddSystem(new PaddleMovementSystem());
        world.AddSystem(new MovementSystem());
        world.AddResource(new Score());
        world.AddResource(new InputState());


        ball = world.EntityCreate();
        world.AddComponent(ball, new Position { Value = new Vector2(640, 360) });
        world.AddComponent(ball, new Velocity { Value = new Vector2(500, 500) });

        paddle1 = world.EntityCreate();
        world.AddComponent(paddle1, new Position { Value = new Vector2(100, 360) });
        world.AddComponent(paddle1, new Velocity { Value = new Vector2(0, 0) });
        world.AddComponent(paddle1, new PlayerControlled { Up = Keys.W, Down = Keys.S, Speed = 425 });

        paddle2 = world.EntityCreate();
        world.AddComponent(paddle2, new Position { Value = new Vector2(1180, 360) });
        world.AddComponent(paddle2, new Velocity { Value = new Vector2(0, 0) });
        world.AddComponent(paddle2, new PlayerControlled { Up = Keys.Up, Down = Keys.Down, Speed = 425 });

        world.AddSystem(new PongRulesSystem(ball, paddle1, paddle2));

        scoreboard = world.EntityCreate();
        world.AddComponent(scoreboard, new Position { Value = new Vector2(1280 / 2, 25 / 2) });

        background = world.EntityCreate();
        world.AddComponent(background, new Position { Value = new Vector2(0, 0) });

    }

    protected override void LoadContent()
    {
        base.LoadContent();
        world.AddComponent(background, new Image2D { Texture = Content.Load<Texture2D>("pong/Background_Grid") });

        world.AddComponent(ball, new Image2D { Texture = Content.Load<Texture2D>("pong/Ball") });
        world.AddComponent(paddle1, new Image2D { Texture = Content.Load<Texture2D>("pong/Paddle_1") });
        world.AddComponent(paddle2, new Image2D { Texture = Content.Load<Texture2D>("pong/Paddle_2") });
        world.AddComponent(scoreboard, new Font { Value = Content.Load<SpriteFont>("Fonts/Hud") });

        world.AddResource(new Screen
        {
            Width = GraphicsDevice.Viewport.Width,
            Height = GraphicsDevice.Viewport.Height
        });
    }

    protected override void Update(GameTime gameTime)
    {
        var input = world.GetResource<InputState>();
        input.PreviousKeyboard = input.Keyboard;
        input.Keyboard = Keyboard.GetState();
        input.GamePad = GamePad.GetState(PlayerIndex.One);

        //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
        if (input.Keyboard.IsKeyDown(Keys.Escape))
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