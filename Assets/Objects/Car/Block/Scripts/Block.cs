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
    /// ������ ������, � ������� ���� �����������
    /// </summary>
    public List<Block> connectedBlocks = new List<Block>();
    /// <summary>
    /// ������ ������, � ������� ���� ����� ��������������
    /// </summary>
    public List<BlockCollider> canConnectTo = new List<BlockCollider>();
    /// <summary>
    /// ���� true - ��� Core ���� ������
    /// </summary>
    public bool isCore = false;
    /// <summary>
    /// ������� ��������� ����� !!! ������ ������ ����� ����� EnterState !!!
    /// </summary>
    public IBlockState currentState;
    /// <summary>
    /// ������ ���� ��������� ��������� �����, ������������� � ������ Awake, ����� ����� ������ �� �����
    /// </summary>
    public Dictionary<Type, IBlockState> allStates;
    public Vector2 position;
    /// <summary>
    /// ��� ���������� ��������� �� Core ���� ������ ������
    /// </summary>
    public Block core;
    /// <summary>
    /// ���� ���������� true, ������ ���� ���� �����������, ���� false - ���
    /// </summary>
    public bool isConnected = false;
    
    public void Awake()
    {
        allStates = new Dictionary<Type, IBlockState>() // ����� �������� ��� ��������� ���������
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
        if (!isConnected) return true;
        if(graph.CanRemove(this)) 
            return true;
        return false;
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
        return true; // TODO: ����������
    }

    public void DisconnectFrom(Block block)
    {
        if (!connectedBlocks.Contains(block))
            return;
        if (!CanDisconnect())
            return; 
        if(graph.Contains(this))
            graph.Remove(this);
        isConnected = false;
        connectedBlocks.Remove(block);
        block.connectedBlocks.Remove(this);
    }

    public void AddReference(Block block)
    {
        connectedBlocks.Add(block);
    }

    public void ConnectTo(BlockCollider block)
    {
         if (!CanConnectTo(block.parentBlock))
            return;
        AddReference(block.parentBlock);
        block.parentBlock.AddReference(this);
        graph.Add(this, connectedBlocks);

        transform.position = new Vector2(block.parentBlock.transform.position.x, block.parentBlock.transform.position.y) + block.refPosition.normalized * 0.645f;
        position = block.refPosition;
        EnterState<Connected>();
        isConnected = true;
        transform.rotation = block.transform.rotation;
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
        {
            EnterState<Drag>();
        }

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
