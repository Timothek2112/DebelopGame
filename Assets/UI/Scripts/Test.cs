using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCollision : MonoBehaviour
{
    [SerializeField] int uiLayer = 8;
    [SerializeField] int blocksLayer = 7;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent<Block>(out var block) && block.CompareState<Drag>())
        {
            block.transform.SetParent(transform);
            block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, transform.position.z - 1);
            block.gameObject.layer = LayerMask.NameToLayer("BlocksUI");
            Debug.Log("Trigger entered: " + collision.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Block>(out var block) && block.CompareState<Drag>())
        {
            block.transform.SetParent(null);
            block.transform.position = new Vector3(block.transform.position.x, block.transform.position.y, 90);
            block.gameObject.layer = LayerMask.NameToLayer("Blocks");
            Debug.Log("Trigger entered: " + collision.gameObject.name);
        }
    }
}
