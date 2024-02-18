using System.Collections;
using System.Collections.Generic;
using DINO;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    #region SerializedFields
    
    [Header("Enemy Spawner Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float rotationEnemy = 0f;
    [SerializeField] private int maxEnemies = 10;
    
    #endregion

    #region private variables
    private int _enemiesCount = 0;

    #endregion
   
    private void Start()
    {
        GameplayController.Instance.OnGameStateChanged += OnGameStateChange;
    }

   
    private IEnumerator SpawnEnemy()
    {
        while (_enemiesCount < maxEnemies)
        {
            GameObject enemy = Instantiate(enemyPrefab, transform.position, Quaternion.Euler(0, rotationEnemy, 0));
            _enemiesCount++;
            enemy.name = "Enemy " + _enemiesCount;
            
            yield return new WaitForSeconds(spawnRate);
        }
    }
    
    
    private void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Countdown:
                break;
            case GameState.Playing:
                StartCoroutine(SpawnEnemy());
                break;
            case GameState.GameOver:
                StopAllCoroutines();
                break;
        }
    }
    
}
