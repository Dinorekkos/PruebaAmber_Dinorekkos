using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTarget : MonoBehaviour
{
    #region SerializedFields

    [SerializeField] private int health = 100;
    [SerializeField] private int damage = 10;

    #endregion

    #region private metods

    public void ReceiveDamage(int damage)
    {
        // Debug.Log("Enemy received damage: ".SetColor("#FB7607") + damage);
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
    #endregion
}
