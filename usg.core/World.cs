using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace usg.core;

public interface ISystem
{
    void Update(SystemContext ctx, GameTime gameTime);
}

public interface IDraw
{
    void Draw(DrawContext ctx, GameTime gameTime);
}

public class World
{
    public EntityManager EntityManager { get; private set; }

    private List<ISystem> systems = new();
    private List<IDraw> drawSystems = new();

    public CommandBuffer Commands = new();

    private readonly Dictionary<Type, object> stores = new();
    private readonly Dictionary<Type, object> resources = new();

    public World()
    {
        EntityManager = new EntityManager();
    }

    public Entity EntityCreate()
    {
        return EntityManager.Create();
    }

    public void EntityDestroy(Entity entity)
    {
        if (!EntityManager.IsAlive(entity))
#if DEBUG
            throw new InvalidOperationException("Entity is not alive.");
#else
            return;
#endif
        foreach (var store in stores.Values)
        {
            ((IStore)store).Remove(entity.Id);
        }
        EntityManager.Destroy(entity.Id);
    }

    public Entity Handle(int id) => new Entity(id, EntityManager.GetGeneration(id));


    public bool HasComponent<T>(Entity entity) where T : struct =>
        Store<T>().Has(entity.Id);

    public ref T GetComponent<T>(Entity entity) where T : struct =>
        ref Store<T>().Get(entity.Id);

    public void RemoveComponent<T>(Entity entity) where T : struct
    {
        if (!EntityManager.IsAlive(entity)) return;
        Store<T>().Remove(entity.Id);
    }

    public SparseSet<T> Store<T>() where T : struct
    {
        if (!stores.TryGetValue(typeof(T), out var store))
            stores[typeof(T)] = store = new SparseSet<T>();

        return (SparseSet<T>)store;
    }
    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        Store<T>().Add(entity.Id, component);
    }

    public void AddResource<T>(T resource) where T : class
    {
        resources[typeof(T)] = resource;
    }

    public T GetResource<T>() where T : class
    {
        if (!resources.TryGetValue(typeof(T), out var resource))
            throw new InvalidOperationException($"Resource of type {typeof(T)} not found.");
        return (T)resource;
    }

    public bool TryGetResource<T>(out T resource) where T : class
    {
        if (resources.TryGetValue(typeof(T), out var res))
        {
            resource = (T)res;
            return true;
        }
        resource = null;
        return false;
    }

    public void AddSystem(ISystem system) => systems.Add(system);
    public void Update(GameTime gameTime)
    {
        foreach (var system in systems)
        {
            system.Update(new SystemContext(this), gameTime);
            Commands.Execute(this);
        }
    }
    public void AddDrawSystem(IDraw drawSystem) => drawSystems.Add(drawSystem);
    public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        foreach (var drawSystem in drawSystems)
        {
            drawSystem.Draw(new DrawContext(this, spriteBatch), gameTime);
        }
    }
}