using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerHealthBar : MonoBehaviour
{
    #region SerializedFields
    
    [SerializeField] private Tower tower;
    [SerializeField] private Slider slider;
    
    
    #endregion

    #region private methods
    
    void Start()
    {
        slider.maxValue = tower.Health;
        slider.value = tower.Health;
        
        tower.OnTowerHealthChanged += UpdateHealthBar;
    }

    private void OnDestroy()
    {
        tower.OnTowerHealthChanged -= UpdateHealthBar;
    }

    private void UpdateHealthBar(int value)
    {
        slider.value = value;
    }
    
    #endregion

}
