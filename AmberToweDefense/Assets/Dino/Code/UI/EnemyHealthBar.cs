using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DINO
{
    public class EnemyHealthBar : MonoBehaviour
    {
        
        #region SerializedFields
        
        [SerializeField] private EnemyTarget enemy;
        [SerializeField] private Slider slider;
        
        #endregion
        
        #region private methods
        void Start()
        {
            slider.maxValue = enemy.Health;
            slider.value = enemy.Health;
            
            enemy.OnEnemyHealthChanged += UpdateHealthBar;
        }
        
        private void OnDestroy()
        {
            enemy.OnEnemyHealthChanged -= UpdateHealthBar;
        }
        private void UpdateHealthBar(int value)
        {
            slider.value = value;
        }
        #endregion
        
    }

}