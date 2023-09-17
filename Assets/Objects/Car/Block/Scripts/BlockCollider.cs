using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public Block parentBlock;
    public Vector2 refPosition;
    public bool isTaken = false;
    public Transform positionForBlock;

    private void Awake()
    {
        parentBlock = transform.parent.GetComponent<Block>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Collider"))
        {
            var collider = collision.GetComponent<BlockCollider>();
            if (!collider.parentBlock.isConnected)
                return;
            if (collider.isTaken) return;
            if (refPosition != -collider.refPosition) return;
            parentBlock.canConnectTo.Add(new Tuple<BlockCollider, BlockCollider>(this, collider));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Collider"))
        {
            var collider = collision.GetComponent<BlockCollider>();

            if (parentBlock.canConnectTo.Contains(new Tuple<BlockCollider, BlockCollider>(this, collider)))
            {
                parentBlock.canConnectTo.Remove(new Tuple<BlockCollider, BlockCollider>(this, collider));
            }
        }
    }
}
