using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Drag : IBlockState
{
    Block block;

    public Drag(Block block)
    {
        this.block = block;
    }

    public void Enter()
    {

    }

    public void Exit()
    {

    }
}

