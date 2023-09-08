using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class IBlockState
{
    public Block block;
    public Rigidbody2D rigidbody;

    public IBlockState(Block block)
    {
        this.block = block;
        rigidbody = block?.GetComponent<Rigidbody2D>();
    }
    public abstract void Enter();
    public abstract void Process(float dTime);
    public abstract void Exit();
}
