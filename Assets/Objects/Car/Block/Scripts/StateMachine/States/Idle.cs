using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Idle : IBlockState
{
    Block block;

    public Idle(Block block)
    {
        this.block = block;
        Debug.Log(block);
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }
}
