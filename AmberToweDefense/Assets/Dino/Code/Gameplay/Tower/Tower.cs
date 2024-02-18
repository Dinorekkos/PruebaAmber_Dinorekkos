using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private int health = 100;
    
    #endregion

    #region public variables

    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnTowerHealthChanged?.Invoke(health);
        }
    }
    
    public Action OnTowerDied;
    public Action<int> OnTowerHealthChanged;

    #endregion

    #region unity methods


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyTarget>())
        {
            EnemyTarget enemy = other.GetComponent<EnemyTarget>();
            ReceiveDamage(enemy.Damage);
        }
    }

    #endregion
    
    #region public Methods
    public void ReceiveDamage(int damage)
    {
        Debug.Log("Tower received damage: ".SetColor("#FB7607") + damage);
        Health -= damage;
        if (health <= 0)
        {
            TowerDied();
        }
    }
    
    public bool IsDead()
    {
        return health <= 0;
    }
    #endregion

    #region private methods

    
    private void TowerDied()
    {
        Debug.Log("Tower died".SetColor("#FB7607"));
        OnTowerDied?.Invoke();  
        // gameObject.SetActive(false);
    }

    #endregion

}
