using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Search;

[Serializable]
public class Vertex
{
    public Block block;
    public List<Vertex> connectedTo = new List<Vertex>();
    
    public Vertex(Block block)
    {
        this.block = block;
    }

    public Vertex(Block block, List<Vertex> connectedTo)
    {
        this.block = block;
        this.connectedTo = connectedTo;
    }
}

public class BlocksGraph : MonoBehaviour
{
    private List<Vertex> vertices = new List<Vertex>();
    private Vertex core;

    private void Awake()
    {
        Block core = GameObject.FindGameObjectWithTag("Core").GetComponent<Block>();
        this.core = Add(core, core.connectedBlocks);
    }

    public Vertex Add(Block block, List<Block> connected) // Не добавлять вершину в граф, если она уже есть
    {
        Vertex vert = vertices.FirstOrDefault(p => p.block == block);
        if (vert == null)
        {
            vert = new Vertex(block);
            vertices.Add(vert);
            vert.connectedTo = new List<Vertex>();
        }
        foreach (var item in connected)
        {
            var vertex = vertices.FirstOrDefault(p => p.block == item);
            AddReference(vert, vertex);
            AddReference(vertex, vert);
        }
        return vert;
    }

    private void AddReference(Vertex from, Vertex to)
    {
        if (from.connectedTo.Contains(to))
            return;
        from.connectedTo.Add(to);
    }

    public bool Contains(Block block)
    {
        if(vertices.FirstOrDefault(p => p.block == block) == null) return false;
        return true;
    }

    public void Remove(Block block)
    {
        if(vertices.FirstOrDefault(p => p.block == block) == null) return;
        foreach(var vert in vertices)
        {
            Vertex finded = vert.connectedTo.FirstOrDefault(p => p.block == block);
            if(finded != null)
                vert.connectedTo.Remove(finded);
        }
        vertices.Remove(vertices.FirstOrDefault(p => p.block == block));
    }

    public bool CanRemove(Block block)
    {
        Vertex blockVert = vertices.FirstOrDefault(p => p.block == block);
        List<Vertex> connected = blockVert.connectedTo;
        List<Vertex> targets = Copy(connected);
        List<Vertex> vertCopy = Copy(vertices);
        vertCopy.Remove(vertCopy.FirstOrDefault(p => p.block == block));
        Queue<Vertex> queue = new Queue<Vertex>();
        List<Vertex> passed = new List<Vertex>();
        queue.Enqueue(core);
        Remove(block);
        while (queue.Count > 0)
        {
            Vertex current = queue.Dequeue();
            if (targets.Contains(current))
            {
                targets.Remove(current);
            }
            if(targets.Count == 0)
            {
                Add(block, block.connectedBlocks);
                return true;
            }
            foreach(var vert in current.connectedTo)
            {
                if (passed.Contains(vert))
                    continue;

                queue.Enqueue(vert);
            }
            passed.Add(current);
        }
        Add(block, block.connectedBlocks);
        return false;
    }

    public List<Vertex> Copy(List<Vertex> list)
    {
        Vertex[] vertArray = new Vertex[list.Count];
        list.CopyTo(vertArray);
        List<Vertex> vertCopy = vertArray.ToList();
        return vertCopy;
    }
}