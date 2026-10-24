using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using usg.core;

namespace Pong;

public class Game : UltraGame
{
    private Texture2D ball;
    private Texture2D paddle1;
    private Texture2D paddle2;
    public Game() : base("Pong", 1920, 1080, false)
    {

    }

    protected override void LoadContent()
    {
        base.LoadContent();
        ball = Content.Load<Texture2D>("pong/Ball");
        paddle1 = Content.Load<Texture2D>("pong/Paddle_1");
        paddle2 = Content.Load<Texture2D>("pong/Paddle_2");
    }

    protected override void Draw(GameTime gameTime)
    {
        base.Draw(gameTime);
        spriteBatch.Begin();
        spriteBatch.Draw(ball, new Vector2(960, 540), Color.White);
        spriteBatch.Draw(paddle1, new Vector2(100, 540), Color.White);
        spriteBatch.Draw(paddle2, new Vector2(1820, 540), Color.White);
        spriteBatch.End();
    }
}
