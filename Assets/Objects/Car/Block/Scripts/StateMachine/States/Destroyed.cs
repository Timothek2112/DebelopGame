using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Destroyed : IBlockState
{
    public Block block;

    public Destroyed(Block block)
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
