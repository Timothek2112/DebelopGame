using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuyItemUI : MonoBehaviour
{
    public BuyItem item;
    [SerializeField] TMP_Text nameText;
    [SerializeField] TMP_Text costText;
    [SerializeField] TMP_Text amountInShopText;
    [SerializeField] TMP_Text currentAmountText;
    float _cost = 0;
    float _amountInShop = 0;
    float _currentAmount = 0;
    float cost
    {
        get { return _cost; }
        set { _cost = value; UpdateTextFields(); }
    }
    float amountInShop
    {
        get { return _amountInShop; }
        set { _amountInShop = value; UpdateTextFields(); }
    }
    float currentAmount
    {
        get { return _currentAmount; }
        set { _currentAmount = value; UpdateTextFields(); }
    }

    public void UpdateTextFields()
    {

    }

    public void SetData(BuyItem item, float cost)
    {
        this.item = item;
        nameText.text = item.block.name;
        this.costText.text = cost.ToString();
        amountInShopText.text = item.amount.ToString();
        currentAmountText.text = "0";
    }

    public void Plus()
    {

    }

    public void Minus()
    {

    }
}
