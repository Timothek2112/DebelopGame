using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BuyMenu : MonoBehaviour
{
    private float _cost = 0;
    public float cost
    {
        get
        {
            return _cost;
        }
        set
        {
            _cost = value;
            UpdateText();
        }
    }

    public List<BuyItem> cart = new List<BuyItem>();
    public List<BuyItem> shop = new List<BuyItem>();
    [SerializeField] List<BlockCostPair> extraPrices = new List<BlockCostPair>();
    [SerializeField] TMP_Text costText;
    [SerializeField] GameObject buyItemUIprefab;
    [SerializeField] Transform parentForItems;

    private void Awake()
    {
        foreach(var item in shop)
        {
            var buyItemUI = Instantiate(buyItemUIprefab);
            buyItemUI.GetComponent<BuyItemUI>().SetData(item, GetCostForBlock(item.block));
            buyItemUI.transform.SetParent(parentForItems, false);
        }
    }

    public void UpdateText()
    {
        costText.text = cost.ToString();
    }

    public void AddToCart(BuyItem item)
    {
        BuyItem buyItem = cart.FirstOrDefault(p => p.block == item.block);
        if(buyItem == null)
        {
            cart.Add(item);
            buyItem = item;
        }
        else
        {
            buyItem.amount += item.amount;
        }
        cost += GetCostForBlock(item.block) * item.amount;
    }

    public void RemoveFromCart(BuyItem item)
    {
        BuyItem buyItem = cart.FirstOrDefault(p => p.block == item.block);
        if (buyItem == null) return;
        buyItem.amount -= item.amount;
        if (buyItem.amount < 0)
        {
            buyItem.amount += item.amount;
            return;
        }
        if(buyItem.amount == 0)
        {
            cart.Remove(buyItem);
        }
        cost -= item.amount * GetCostForBlock(item.block);
    }

    public float GetCostForBlock(Block block)
    {
        BlockCostPair extraPrice = extraPrices.FirstOrDefault(p => p.block == block);
        float extra = 0;
        if (extraPrice != null) extra = (block.GetCost() / 100) * extraPrice.cost;
        return block.GetCost() + extra;
    }
}
