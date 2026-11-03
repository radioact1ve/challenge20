using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Graphics;
using MonoGame.Extended.Particles;
namespace usg.core;

public readonly struct SystemContext
{
    private readonly World world;
    internal SystemContext(World w) => world = w;
    public T Resource<T>() where T : class => world.GetResource<T>();
    public Entity Handle(int id) => world.Handle(id);
    public SparseSet<T> Store<T>() where T : struct => world.Store<T>();
    public ref T Get<T>(Entity e) where T : struct => ref world.GetComponent<T>(e);
    public bool Has<T>(Entity e) where T : struct => world.HasComponent<T>(e);
    public Entity Create() => world.EntityCreate();
    public CommandBuffer Commands => world.Commands;
}

public readonly struct DrawContext
{
    private readonly World world;
    public readonly SpriteBatch SpriteBatch;
    internal DrawContext(World w, SpriteBatch sb) { world = w; SpriteBatch = sb; }
    public T Resource<T>() where T : class => world.GetResource<T>();
    public SparseSet<T> Store<T>() where T : struct => world.Store<T>();
    public ref readonly T Get<T>(Entity e) where T : struct => ref world.GetComponent<T>(e);
    public bool Has<T>(Entity e) where T : struct => world.HasComponent<T>(e);
}

public class DrawImage : IDraw
{
    public void Draw(DrawContext ctx, GameTime gameTime)
    {
        var sprites = ctx.Store<Image2D>();
        var positions = ctx.Store<Position>();

        foreach (int e in sprites.Entities)
        {
            if (!positions.Has(e)) continue;
            ctx.SpriteBatch.Draw(sprites.Get(e).Texture, positions.Get(e).Value, Color.White);
        }
    }
}

public class DrawSprite : IDraw
{
    public void Draw(DrawContext ctx, GameTime gameTime)
    {
        var animations = ctx.Store<Animation2D>();
        var positions = ctx.Store<Position>();

        foreach (int e in animations.Entities)
        {
            if (!positions.Has(e)) continue;
            var sprite = animations.Get(e).Sprite;
            ctx.SpriteBatch.Draw(sprite, positions.Get(e).Value, 0, new Vector2(5));

        }
    }
}