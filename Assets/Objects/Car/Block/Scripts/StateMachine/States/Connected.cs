using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Connected : IBlockState
{
    public Connected(Block block) : base(block)
    {
    }

    public override void Enter()
    {
        block.transform.SetParent(GameObject.FindGameObjectWithTag("CarBlocks").transform);
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
    }

    public override void Process(float dTime)
    {

    }

    public override void Exit()
    {
        block.transform.SetParent(null);
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
