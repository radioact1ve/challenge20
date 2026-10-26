using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace usg.core;

public class Core : Game
{
    public Core Instance { get; private set; }
    public GraphicsDeviceManager Graphics { get; private set; }
    public new GraphicsDevice GraphicsDevice { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public new ContentManager Content { get; private set; }

    public Core(string title, int width, int height, bool isFullScreen) : base()
    {
        if (Instance != null)
        {
            throw new InvalidOperationException("Only one instance of UltraGame is allowed.");
        }

        Instance = this;
        Graphics = new GraphicsDeviceManager(this);
        Window.Title = title;
        Graphics.PreferredBackBufferWidth = width;
        Graphics.PreferredBackBufferHeight = height;
        Graphics.IsFullScreen = isFullScreen;
        Graphics.ApplyChanges();
        Content = base.Content;
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        base.Initialize();
    }

    protected override void LoadContent()
    {
        GraphicsDevice = base.GraphicsDevice;
        SpriteBatch = new SpriteBatch(GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
