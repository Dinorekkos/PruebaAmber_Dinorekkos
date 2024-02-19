using System;
using System.Collections;
using System.Collections.Generic;
using DINO;
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
            enemy.SetDeadByTower();
            ReceiveDamage(enemy.Damage);
        }
    }

    #endregion
    
    #region public Methods
    public void ReceiveDamage(int damage)
    {
        Health -= damage;
        GameplayController.Instance.CheckEnemiesRemaining();
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
        OnTowerDied?.Invoke();  
        GameplayController.Instance.ChangeGameState(GameState.GameOver);
    }

    #endregion

}
