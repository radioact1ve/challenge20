using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Graphics;
using System;
using usg.core;

namespace flappy;

public class Game : Core
{
    private readonly World world;
    private Entity bird;
    private Entity background;
    public Game() : base("flapy", 1280, 720, false)
    {
        world = new World();

        world.AddSystem(new AnimationSystem());
        world.AddDrawSystem(new DrawSprite());
        world.AddResource(new InputState());

        bird = world.EntityCreate();
        world.AddComponent(bird, new Bird());
        world.AddComponent(bird, new Position { Value = new Vector2(50, 360) });
        world.AddComponent(bird, new Velocity { Value = new Vector2(0, 0) });

        background = world.EntityCreate();
        world.AddComponent(background, new Position { Value = new Vector2(0, 0) });
    }

    protected override void LoadContent()
    {
        base.LoadContent();

        Texture2DAtlas atlas = Content.Load<Texture2DAtlas>("flappy/Bird1-1");
        SpriteSheet spriteSheet = new SpriteSheet("SpriteSheet/flappy", atlas);
        TimeSpan duration = TimeSpan.FromSeconds(0.1);
        spriteSheet.DefineAnimation("flap", builder =>
        {
            builder.IsLooping(true)
                   .AddFrame("flap-00", duration)
                   .AddFrame("flap-01", duration)
                   .AddFrame("flap-02", duration)
                   .AddFrame("flap-03", duration);
        });
        world.AddComponent(bird, new Animation2D { Sprite = new AnimatedSprite(spriteSheet, "flap") });
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