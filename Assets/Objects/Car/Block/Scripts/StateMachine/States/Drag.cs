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
    public GameObject car;

    public Drag(Block block) : base(block) { }

    public override void Enter()
    {
        car = GameObject.FindGameObjectWithTag("Car");
        block.DisconnectAll();
    }

    public override void Process(float dTime)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        block.transform.rotation = car.transform.rotation;
        if (FindDistanceToNearestBlock(mousePos) > 2)
        {
            rigidbody.velocity = (new Vector3(mousePos.x, mousePos.y, block.transform.position.z) - block.transform.position) * dragSpeed;
        }
        else
        {
            block.transform.position = FindNearestCollider(mousePos);
        }
    }

    public override void Exit()
    {
        
    }

    private float FindDistanceToNearestBlock(Vector2 mousePos)
    {
        List<Block> connectedBlocks = GameObject.FindObjectsOfType<Block>().ToList();
        connectedBlocks.RemoveAll(p => p.isConnected == false);
        float minDistance = float.MaxValue;
        foreach(var block in connectedBlocks)
        {
            float dist = Vector2.Distance(block.transform.position, mousePos);
            if(dist < minDistance)
                minDistance = dist;
        }
        return minDistance;
    }

    private Vector2 FindNearestCollider(Vector2 mousePos)
    {
        GameObject[] allColliders = GameObject.FindGameObjectsWithTag("Collider");
        float minDistance = float.MaxValue;
        Vector2 minPosition = Vector2.zero;
        foreach(var collider in allColliders)
        {
            BlockCollider blockCollider = collider.GetComponent<BlockCollider>();
            if (!blockCollider.parentBlock.isConnected) continue;
            if (blockCollider.isTaken) continue;
            if(Vector2.Distance(mousePos, blockCollider.positionForBlock.position) < minDistance)
            {
                minDistance = Vector2.Distance(mousePos, blockCollider.positionForBlock.position);
                minPosition = collider.GetComponent<BlockCollider>().positionForBlock.position;
            }
        }
        return minPosition;
    }
}

