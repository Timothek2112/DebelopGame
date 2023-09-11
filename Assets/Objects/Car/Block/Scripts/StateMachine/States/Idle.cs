using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Idle : IBlockState
{
    public float stopFactor = 0.99f;
    public Idle(Block block) : base(block) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Process(float dTime)
    {
        rigidbody.velocity *= stopFactor;
    }
}
