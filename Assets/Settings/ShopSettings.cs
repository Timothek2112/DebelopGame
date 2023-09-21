using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BlockCostPair
{
    public Block block;
    public float cost;
}

[CreateAssetMenu(fileName = "Настройки магазина", menuName = "ScriptableObjects/Настройки магазина", order = 1)]
public class ShopSettings : ScriptableObject
{
    public List<BlockCostPair> costs = new List<BlockCostPair>();
}
