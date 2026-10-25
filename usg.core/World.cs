using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace usg.core;

public interface IComponentStore<T> where T : struct
{
    void Add(int entity, T component);
    ref T Get(int entity);
    bool Has(int entity);
    bool Remove(int entity);
}

public interface ISystem
{
    void Update(World world, GameTime gameTime);
}

public interface IDraw
{
    void Draw(World world, SpriteBatch spriteBatch, GameTime gameTime);
}

public class World
{
    public EntityManager EntityManager { get; private set; }

    private List<Entity> players = new();
    private List<ISystem> systems = new();
    private List<IDraw> drawSystems = new();

    private readonly Dictionary<Type, object> stores = new();

    public World()
    {
        EntityManager = new EntityManager();
    }

    public Entity EntityPlayer()
    {
        var entity = EntityManager.Create();
        players.Add(entity);
        return entity;
    }

    public Entity EntityCreate()
    {
        return EntityManager.Create();
    }

    public void EntityDestroy(Entity entity)
    {
        EntityManager.Destroy(entity.Id);
    }

    public ReadOnlySpan<int> Entities<T>() where T : struct =>
        GetStore<T>().Entities;

    public bool HasComponent<T>(int entity) where T : struct =>
        GetStore<T>().Has(entity);

    public ref T GetComponent<T>(int entity) where T : struct =>
        ref GetStore<T>().Get(entity);

    public void RemoveComponent<T>(int entity) where T : struct =>
        GetStore<T>().Remove(entity);

    private SparseSet<T> GetStore<T>() where T : struct
    {
        if (!stores.TryGetValue(typeof(T), out var store))
            stores[typeof(T)] = store = new SparseSet<T>();

        return (SparseSet<T>)store;
    }
    public void AddComponent<T>(Entity entity, T component) where T : struct
    {
        GetStore<T>().Add(entity.Id, component);
    }

    public void AddSystem(ISystem system) => systems.Add(system);
    public void Update(GameTime gameTime)
    {
        foreach (var system in systems)
        {
            system.Update(this, gameTime);
        }
    }
    public void AddDrawSystem(IDraw drawSystem) => drawSystems.Add(drawSystem);
    public void Draw(GameTime gameTime)
    {
        foreach (var drawSystem in drawSystems)
        {
            drawSystem.Draw(this, Core.SpriteBatch, gameTime);
        }
    }

    public void Exit()
    {
        Core.Instance.Exit();
    }
}