using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuyItem 
{
    public Block block;
    public GameObject prefab;
    public int amount;

    public BuyItem(Block block, GameObject prefab, int amount)
    {
        this.block = block;
        this.prefab = prefab;
        this.amount = amount;
    }
}
