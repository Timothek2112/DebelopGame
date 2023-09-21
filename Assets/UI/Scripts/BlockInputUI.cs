using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockInputUI : MonoBehaviour
{
    [SerializeField] int uiLayer = 8;
    [SerializeField] int blocksLayer = 7;
    [SerializeField] IOnBlockEnteredUI onBlockEnteredUI;
    [SerializeField] private List<Block> blocksInUI = new List<Block>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Block>(out var block) && block.CompareState<Drag>())
        {
            Add(block);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Block>(out var block))
        {
            Remove(block);
        }
    }

    public void Add(Block block)
    {
        block.transform.SetParent(transform);
        block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, transform.position.z - 1);
        block.gameObject.layer = LayerMask.NameToLayer("BlocksUI");
        onBlockEnteredUI.OnBlockEntered(block);
        blocksInUI.Add(block);
    }

    public void Remove(Block block)
    {
        if (!Contains(block)) return;
        block.transform.SetParent(null);
        block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, 90);
        block.gameObject.layer = LayerMask.NameToLayer("Blocks");
        onBlockEnteredUI.OnBlockExited(block);
        blocksInUI.Remove(block);
    }

    public bool Contains(Block block)
    {
        return blocksInUI.Contains(block);
    }

    public void DeleteAllBlocks()
    {
        while (blocksInUI.Count > 0)
        {
            Block block = blocksInUI[0];
            Remove(block);
            Destroy(block.gameObject);
        }
    }
}
