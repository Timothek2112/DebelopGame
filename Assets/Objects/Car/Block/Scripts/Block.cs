using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;

[RequireComponent(typeof(BoxCollider2D), typeof(Rigidbody2D))]
public class Block : MonoBehaviour
{
    GameObject car;
    BlocksGraph graph;
    /// <summary>
    /// Список блоков, к которым блок присоединен
    /// </summary>
    public List<Block> connectedBlocks = new List<Block>();
    public List<Tuple<BlockCollider, BlockCollider>> connectedColliders = new List<Tuple<BlockCollider, BlockCollider>>();
    /// <summary>
    /// Список блоков, к котором блок может присоединиться
    /// </summary>
    public List<Tuple<BlockCollider, BlockCollider>> canConnectTo = new List<Tuple<BlockCollider, BlockCollider>>();
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
    public List<BlockCollider> thisColliders = new List<BlockCollider>();

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
        car = GetCar();
    }

    private GameObject GetCar()
    {
        return GameObject.FindGameObjectWithTag("Car");
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
        if (!isConnected) return true;
        if (graph.CanRemove(this))
            return true;
        return false;
    }

    public void DisconnectAll()
    {
        while (connectedBlocks.Count > 0)
        {
            DisconnectFrom(connectedBlocks[0]);
        }
    }

    public bool CanConnectTo(Block block)
    {
        if (connectedBlocks.Contains(block))
            return false;
        return true;
    }

    public void DisconnectFrom(Block block)
    {
        if (!connectedBlocks.Contains(block))
            return;
        if (!CanDisconnect())
            return;
        if (graph.Contains(this))
            graph.Remove(this);
        foreach (var collider in block.thisColliders)
        {
            for (int i = 0; i < connectedColliders.Count; i++)
            {
                if (connectedColliders[i].Item2 == collider)
                {
                    connectedColliders[i].Item1.isTaken = false;
                    connectedColliders[i].Item2.isTaken = false;
                    connectedColliders.RemoveAt(i);
                }
            }
        }
        foreach (var collider in thisColliders)
        {
            for (int i = 0; i < block.connectedColliders.Count; i++)
            {
                if (block.connectedColliders[i].Item2 == collider)
                {
                    block.connectedColliders[i].Item1.isTaken = false;
                    block.connectedColliders[i].Item2.isTaken = false; 
                    block.connectedColliders.RemoveAt(i);
                }
            }
        }
        isConnected = false;
        connectedBlocks.Remove(block);
        block.connectedBlocks.Remove(this);
    }

    public void AddReference(Block block)
    {
        connectedBlocks.Add(block);
    }

    public void ConnectToCanConnect()
    {
        if (canConnectTo.Count > 0)
        {
            foreach (var collider in canConnectTo)
            {
                ConnectTo(collider);
            }
        }
    }

    public void ConnectTo(Tuple<BlockCollider, BlockCollider> colliders)
    {
        if (!CanConnectTo(colliders.Item2.parentBlock))
            return;
        AddReference(colliders.Item2.parentBlock);
        colliders.Item2.parentBlock.AddReference(this);
        graph.Add(this, connectedBlocks);
        position = colliders.Item2.refPosition;
        EnterState<Connected>();
        isConnected = true;
        colliders.Item2.isTaken = true;
        colliders.Item1.isTaken = true;
        connectedColliders.Add(colliders);
        colliders.Item2.parentBlock.connectedColliders.Add(new Tuple<BlockCollider, BlockCollider>(colliders.Item2, colliders.Item1));
        transform.rotation = colliders.Item2.transform.rotation;
        transform.position = new Vector2(colliders.Item2.positionForBlock.position.x, colliders.Item2.positionForBlock.position.y);
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
        if (currentState?.GetType() == typeof(Idle))
            EnterState<Drag>();
        if (currentState.GetType() == typeof(Connected) && CanDisconnect())
        {
            EnterState<Drag>();
        }

    }

    public void OnMouseUp()
    {
        if (isCore) return;
        if (currentState.GetType() == typeof(Drag))
            EnterState<Idle>();
        ConnectToCanConnect();
    }
}
