using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace usg.core;

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


public class LazyEscapeSystem : ISystem
{
    public void Update(World world, GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            world.Exit();
    }
}