using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class IOnBlockEnteredUI : MonoBehaviour
{
    public abstract void OnBlockEntered(Block block);
    public abstract void OnBlockExited(Block block);
}

public class SellMenu : IOnBlockEnteredUI
{
    public float sum = 0;
    [SerializeField] TMP_Text sumContainer;
    [SerializeField] ShopSettings shopSettings;

    public override void OnBlockEntered(Block block)
    {
        float cost = shopSettings.costs.FirstOrDefault(p => p.block == block.type).cost;
        sum += cost;
        UpdateCounter();
    }

    public override void OnBlockExited(Block block)
    {
        float cost = shopSettings.costs.FirstOrDefault(p => p.block == block.type).cost;
        sum -= cost;
        UpdateCounter();
    }

    public void UpdateCounter()
    {
        sumContainer.text = sum.ToString();
    }

    
}
