using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Destroyed : IBlockState
{
    public Destroyed(Block block) : base(block) { }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Process(float dTime)
    {

    }
}
