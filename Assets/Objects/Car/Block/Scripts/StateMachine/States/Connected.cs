using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Connected : IBlockState
{
    Rigidbody2D body;
    public Connected(Block block) : base(block)
    {
    }

    public override void Enter()
    {
        body = block.GetComponent<Rigidbody2D>(); 
        block.transform.SetParent(GameObject.FindGameObjectWithTag("CarBlocks").transform);
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
    }

    public override void Process(float dTime)
    {
        body.velocity = new Vector2(0,0);
    }

    public override void Exit()
    {
        block.transform.SetParent(null);
        block.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
    }
}
