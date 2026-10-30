using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using usg.core;

namespace flappy;

public class Game : Core
{
    private readonly World world;
    private Entity bird;
    public Game() : base("flapy", 1280, 720, false)
    {
        world = new World();

        world.AddDrawSystem(new DrawSystem());
        world.AddResource(new InputState());


        bird = world.EntityCreate();
        world.AddComponent(bird, new Bird());
        world.AddComponent(bird, new Position { Value = new Vector2(50, 360) });
        world.AddComponent(bird, new Velocity { Value = new Vector2(0, 0) });
    }

    protected override void LoadContent()
    {
        base.LoadContent();
        world.AddComponent(bird, new Image2D { Texture = Content.Load<Texture2D>("flappy/Player/StyleBird1/Bird1-1") });
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