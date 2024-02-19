using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI currencyText;
    [SerializeField] private GameObject currency;
    
    void Start()
    {
        CurrencyManager.Instance.OnCurrencyChanged += UpdateCurrencyUI;
        
        UpdateCurrencyUI(CurrencyManager.Instance.Currency);
    }

    private void UpdateCurrencyUI(int obj)
    {
        currencyText.text = obj.ToString();
    }
}
