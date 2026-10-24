using System;
using System.Collections.Generic;

namespace usg.core;

public sealed class SparseSet<T> where T : struct
{
    private int[] sparse = new int[1024];
    private int[] dense = new int[1024];
    private T[] data = new T[1024];
    private int count;

    public SparseSet()
    {
        Array.Fill(sparse, -1);
    }

    public ReadOnlySpan<int> Entities => dense.AsSpan(0, count);
    public Span<T> Components => data.AsSpan(0, count);

    public bool Has(int entity)
    {
        int i = sparse[entity];
        return i != -1 && i < count && dense[i] == entity;
    }

    public ref T Get(int entity)
    {
        if (!Has(entity))
            throw new KeyNotFoundException($"Entity {entity} does not have component of type {typeof(T)}.");

        return ref data[sparse[entity]];
    }

    public void Add(int entity, T component)
    {
        if (Has(entity)) return;
        EnsureCapacityForEntity(entity);

        int i = count++;
        sparse[entity] = i;
        dense[i] = entity;
        data[i] = component;
    }

    public bool Remove(int entity)
    {
        if (entity < 0 || entity >= sparse.Length) return false;
        if (!Has(entity)) return false;

        int i = sparse[entity];
        int last = --count;

        if (i != last)
        {
            int lastEntity = dense[last];

            dense[i] = lastEntity;
            data[i] = data[last];
            sparse[lastEntity] = i;
        }

        // mark entity as invalid
        sparse[entity] = -1;

        return true;
    }

    private void EnsureCapacityForEntity(int entity)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(entity);
        if (entity < sparse.Length) return;

        int newSize = sparse.Length;
        while (newSize <= entity) newSize *= 2;
        Array.Resize(ref sparse, newSize);
        Array.Resize(ref dense, newSize);
        Array.Resize(ref data, newSize);

        // initialize new sparse slots to -1
        for (int i = sparse.Length / 2; i < sparse.Length; i++) sparse[i] = -1;
    }
}
