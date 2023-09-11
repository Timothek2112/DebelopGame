using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Drag : IBlockState
{
    public float dragSpeed = 15;

    public Drag(Block block) : base(block) { }

    public override void Enter()
    {
        block.DisconnectAll();
    }

    public override void Process(float dTime)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rigidbody.velocity =  (new Vector3(mousePos.x, mousePos.y, block.transform.position.z) - block.transform.position) * dragSpeed; 
    }

    public override void Exit()
    {
        
    }
}

