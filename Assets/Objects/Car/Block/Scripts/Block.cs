using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    public List<Block> connectedBlocks = new List<Block>();
    public IBlockState currentState;
    public Dictionary<Type, IBlockState> allStates;
    public Vector2 position;
    public Block core;
    
    public void Awake()
    {
        allStates = new Dictionary<Type, IBlockState>() // Здесь задаются все возможные состояния
        {
            { typeof(Idle), new Idle(this) },
            { typeof(Drag), new Drag(this) },
            { typeof(Destroyed), new Destroyed(this) },
        };
        EnterState<Idle>();
        core = GameObject.FindGameObjectWithTag("Core").GetComponent<Block>();
    }

    public void Update()
    {
        currentState.Process(Time.deltaTime);
    }

    public void DisconnectAll()
    {
        foreach(var block in connectedBlocks)
        {
            this.DisconnectFrom(block);
        }
    }

    public void DisconnectFrom(Block block)
    {
        if (!connectedBlocks.Contains(block))
            return;
        connectedBlocks.Remove(block);
        block.connectedBlocks.Remove(this);
    }

    public void ConnectTo(Block block)
    {
        if (connectedBlocks.Contains(block))
            return;
        connectedBlocks.Add(block);
        block.connectedBlocks.Add(this);
    }

    public void EnterState<TState>() where TState : IBlockState
    {
        if (!allStates.TryGetValue(typeof(TState), out IBlockState state))
            return;
        
        currentState?.Exit();
        currentState = state;
        state.Enter();
    }

    public void OnMouseDown()
    {
        EnterState<Drag>();
        Debug.Log("Drag");
    }
}
