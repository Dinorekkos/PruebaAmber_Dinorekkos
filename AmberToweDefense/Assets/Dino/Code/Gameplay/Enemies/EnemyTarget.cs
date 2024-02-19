using System;
using System.Collections;
using System.Collections.Generic;
using DINO;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 10;

    #endregion

    #region public variables

    public int Damage
    {
        get => damage;
    }
    public int Health
    {
        get => health;
        set
        {
            health = value;
            OnEnemyHealthChanged?.Invoke(health);
        }
    }

    public Action<int> OnEnemyHealthChanged;
    
    #endregion
    
    #region private metods

    public bool IsDead()
    {
        return health <= 0;
    }
    public void SetDeadByTower()
    {
        health = 0;
    }
    public void ReceiveDamage(int damage)
    {
        Health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
        CurrencyManager.Instance.AddCurrency(5);
        GameplayController.Instance.CheckEnemiesRemaining();
    }
    #endregion
}
