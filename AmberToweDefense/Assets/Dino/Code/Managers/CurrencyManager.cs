using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] private int initialCurrency = 10;
    
    public int Currency { get; private set; }
    public static CurrencyManager Instance { get; private set; }
    
    public Action<int> OnCurrencyChanged;

    private void Awake()
    {
        Initialize();
    }
    
    private void Initialize()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Currency = initialCurrency;
    }

    public bool CanAfford(int amount)
    {
        return Currency >= amount;
    }
    
    
    public void AddCurrency(int amount)
    {
        Currency += amount;
        OnCurrencyChanged?.Invoke(Currency);
    }
    
    public void SpendCurrency(int amount)
    {
        if (Currency >= amount)
        {
            Currency -= amount;
            OnCurrencyChanged?.Invoke(Currency);
        }
    }
}
