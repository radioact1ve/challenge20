using System;
using System.Collections.Generic;

namespace usg.core;

public sealed class CommandBuffer
{
    private readonly List<Action<World>> commands = new();

    public void Add<T>(Entity e, T c) where T : struct => commands.Add(w => w.AddComponent(e, c));
    public void Remove<T>(Entity e) where T : struct => commands.Add(w => w.RemoveComponent<T>(e));
    public void Destroy(Entity e) => commands.Add(w => w.EntityDestroy(e));

    public void Execute(World world)
    {
        foreach (var command in commands)
        {
            command(world);
        }
        commands.Clear();
    }

}