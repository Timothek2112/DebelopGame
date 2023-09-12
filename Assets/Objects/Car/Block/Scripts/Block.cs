using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    BlocksGraph graph;
    /// <summary>
    /// Список блоков, к которым блок присоединен
    /// </summary>
    public List<Block> connectedBlocks = new List<Block>();
    /// <summary>
    /// Список блоков, к котором блок может присоединиться
    /// </summary>
    public List<BlockCollider> canConnectTo = new List<BlockCollider>();
    /// <summary>
    /// Если true - это Core блок машины
    /// </summary>
    public bool isCore = false;
    /// <summary>
    /// Текущее состояние блока !!! Менять только через метод EnterState !!!
    /// </summary>
    public IBlockState currentState;
    /// <summary>
    /// Список всех возможных состояний блока, присваивается в методе Awake, после этого менять не нужно
    /// </summary>
    public Dictionary<Type, IBlockState> allStates;
    public Vector2 position;
    /// <summary>
    /// Эта переменная ссылается на Core блок машины игрока
    /// </summary>
    public Block core;
    /// <summary>
    /// Если переменная true, значит этот блок присоединен, если false - нет
    /// </summary>
    public bool isConnected = false;
    
    public void Awake()
    {
        allStates = new Dictionary<Type, IBlockState>() // Здесь задаются все возможные состояния
        {
            { typeof(Idle), new Idle(this) },
            { typeof(Drag), new Drag(this) },
            { typeof(Destroyed), new Destroyed(this) },
            { typeof(Connected), new Connected(this) },
        };
        EnterState<Idle>();
        core = GetCore();
        graph = GetGraph();
    }

    private Block GetCore()
    {
        if (isCore)
            return this;
        else
            return GameObject.FindGameObjectWithTag("Core").GetComponent<Block>();
    }

    private BlocksGraph GetGraph()
    {
        return GameObject.FindGameObjectWithTag("Car").GetComponent<BlocksGraph>();
    }

    public void Update()
    {
        currentState.Process(Time.deltaTime);
    }

    public bool CanDisconnect()
    {
        return true; // TODO: Реализация
    }

    public void DisconnectAll()
    {
        while(connectedBlocks.Count > 0)
        {
            DisconnectFrom(connectedBlocks[0]);
        }
    }

    public bool CanConnectTo(Block block)
    {
        if (connectedBlocks.Contains(block))
            return false;
        return true; // TODO: Реализация
    }

    public void DisconnectFrom(Block block)
    {
        if (!connectedBlocks.Contains(block))
            return;
        if (!CanDisconnect())
            return;
        isConnected = false;
        graph.Remove(this);
        connectedBlocks.Remove(block);
        block.connectedBlocks.Remove(this);
    }

    public void ConnectTo(BlockCollider block)
    {
         if (!CanConnectTo(block.parentBlock))
            return;
        connectedBlocks.Add(block.parentBlock);
        block.parentBlock.connectedBlocks.Add(this);
        transform.position = new Vector2(block.parentBlock.transform.position.x, block.parentBlock.transform.position.y) + block.refPosition * 0.645f;
        position = block.refPosition;
        EnterState<Connected>();
        isConnected = true;
        graph.Add(this, connectedBlocks);
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
        if (isCore) return;
        if(currentState?.GetType() == typeof(Idle))
            EnterState<Drag>();
        if (currentState.GetType() == typeof(Connected) && CanDisconnect())
            EnterState<Drag>();
        if (isConnected)
            Debug.Log(graph.CanRemove(this));
    }

    public void OnMouseUp()
    {
        if (isCore) return;
        if (currentState.GetType() == typeof(Drag))
            EnterState<Idle>();
        if (canConnectTo.Count > 0)
        {
            foreach (var collider in canConnectTo)
            {
                ConnectTo(collider);
            }
        }
    }
}
