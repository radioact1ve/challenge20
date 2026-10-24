using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace usg.core;

public class DrawSystem : IDraw
{
    public void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime)
    {
        //foreach (int e in world.Entities<OldSpriteV1>())
        //{
        //    if (!world.HasComponent<Position>(e))
        //        continue;

        //    ref var transform = ref world.GetComponent<Position>(e);
        //    ref var sprite = ref world.GetComponent<OldSpriteV1>(e);

        //    spriteBatch.Draw(sprite.AnimatedSprite, transform.Value, 0, new Vector2(sprite.Scale));
        //}
    }
}