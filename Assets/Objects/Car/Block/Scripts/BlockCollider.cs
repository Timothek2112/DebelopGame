using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockCollider : MonoBehaviour
{
    public Block parentBlock;
    public Vector2 refPosition;

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
            parentBlock.canConnectTo.Add(collider);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Collider"))
        {
            var collider = collision.GetComponent<BlockCollider>();

            if (parentBlock.canConnectTo.Contains(collider))
            {
                parentBlock.canConnectTo.Remove(collider);
            }
        }
    }
}
