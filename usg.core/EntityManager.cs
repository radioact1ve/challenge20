
using System.Collections.Generic;

namespace usg.core;

public struct Entity
{
    public int Id;
    public int Generation;
}

public class EntityManager
{
    private int nextId = 0;
    private Stack<int> freeIds = new();
    private List<int> generations = new();

    public Entity Create()
    {
        int id = freeIds.Count > 0 ? freeIds.Pop() : nextId++;
        if (id == generations.Count)
            generations.Add(0);

        return new Entity { Id = id, Generation = generations[id] };
    }

    public int GetGeneration(int id)
    {
        return generations[id];
    }

    // Generally when to check if an entity is alive:
    // Event boundaries
    // Commands
    // External references
    // Debug asserts
    // IsAlive should be used if entity is stored/accessed/delayed across frames 
    // within the same frame where entity is created/destroyed, it is not necessary
    public bool IsAlive(Entity e)
    {
        return e.Id < generations.Count &&
               generations[e.Id] == e.Generation;
    }

    public void Destroy(int id)
    {
        generations[id]++;
        freeIds.Push(id);
    }
}