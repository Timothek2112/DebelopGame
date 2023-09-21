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
    private float _sum = 0;
    public float sum { 
        get
        {
            return _sum;
        }
        set
        {
            _sum = value;
            UpdateCounter();
        }
    }
    [SerializeField] List<BlockCostPair> extraPrices = new List<BlockCostPair>();
    [SerializeField] TMP_Text sumContainer;
    [SerializeField] BlockInputUI blockInputUI;

    public override void OnBlockEntered(Block block)
    {
        float cost = GetCostForBlock(block);
        sum += cost;
    }

    public override void OnBlockExited(Block block)
    {
        float cost = GetCostForBlock(block);
        sum -= cost;
    }

    public void UpdateCounter()
    {
        sumContainer.text = sum.ToString();
    }

    public void OnSellClick()
    {
        Wallet playerWallet = GameObject.FindGameObjectWithTag("Car").GetComponent<Wallet>();
        playerWallet.PutMoney(sum);
        blockInputUI.DeleteAllBlocks();
        sum = 0;
    }
    public float GetCostForBlock(Block block)
    {
        BlockCostPair extraPrice = extraPrices.FirstOrDefault(p => p.block == block);
        float extra = 0;
        if (extraPrice != null) extra = (block.GetCost() / 100) * extraPrice.cost;
        return block.GetCost() + extra;
    }
}
