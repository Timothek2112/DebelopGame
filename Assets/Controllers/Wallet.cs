using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallet : MonoBehaviour
{
    [SerializeField] private float money = 0;
    [SerializeField] private bool isInfinite = false;

    public bool IsEnoughMoney(float amount)
    {
        if (isInfinite) return true;
        return money >= amount;
    }

    public void TakeMoney(float amount)
    {
        if (!IsEnoughMoney(amount)) return;
        money -= amount;
    }

    public void PutMoney(float amount)
    {
        money += amount;
    }
}
