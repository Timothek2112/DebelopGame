using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor.Search;

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
    public List<Vertex> vertices = new List<Vertex>();

    public void Add(Block block, List<Block> connected)
    {
        var vert = new Vertex(block);
        foreach(var item in connected)
        {
            var vertex = vertices.FirstOrDefault(p => p.block == item);
            if (vertex == null)
            {
                vertex = new Vertex(block);
            }
            vert.connectedTo.Add(vertex);
        }
    }

    public void Remove(Block block)
    {
        foreach(var vert in vertices)
        {
            vert.connectedTo.Remove(vert.connectedTo.FirstOrDefault(p => p.block == block));
        }
        vertices.Remove(vertices.FirstOrDefault(p => p.block == block));
    }

    public bool CanRemove(Block block)
    {
        List<Vertex> targets = Copy(vertices.FirstOrDefault(p => p.block == block).connectedTo);
        List<Vertex> vertCopy = Copy(vertices);
        Queue<Vertex> queue = new Queue<Vertex>();
        List<Vertex> passed = new List<Vertex>();
        queue.Enqueue(vertCopy[0]);
        while (queue.Count > 0)
        {
            Vertex current = queue.Dequeue();
            if (targets.Contains(current))
            {
                targets.Remove(current);
            }
            if(targets.Count == 0)
            {
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