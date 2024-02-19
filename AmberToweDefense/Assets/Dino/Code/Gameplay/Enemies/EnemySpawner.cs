using System;
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
    [SerializeField] private int maxEnemies = 3;
    [SerializeField] private int maxWave = 3;
    
    #endregion

    #region private variables
    private int _enemiesCount = 0;
    private int _waveCount = 0;
    

    
    #endregion

    #region public variables

    public int CurrentWave => _waveCount;
    public int MaxWave => maxWave;

    public Action<int> OnWaveChange;
    #endregion
    public static EnemySpawner Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameplayController.Instance.OnGameStateChanged += OnGameStateChange;
    }

    private void HandleWaveEnemy()
    {
        _waveCount++;
        OnWaveChange?.Invoke(_waveCount);
        UpdateMaxEnemies();
    }
    private void UpdateMaxEnemies()
    {
        switch (_waveCount)
        {
            case 1:
                maxEnemies = 3;
                break;
            case 2:
                maxEnemies = 5;
                break;
            case 3:
                maxEnemies = 7;
                break;
        }
    }
    private void OnStartGame()
    {
        HandleWaveEnemy();
        StartCoroutine(SpawnEnemy());
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

        if (_enemiesCount >= maxEnemies)
        {
            _enemiesCount = 0;
            GameplayController.Instance.CheckEnemiesRemaining();
            Debug.Log("Wave ".SetColor("#F6FE44") + _waveCount + " is over");
        }
        
    }
    
    
    private void OnGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Preparing:
                StopAllCoroutines();
                break;
            case GameState.Playing:
                OnStartGame();
                break;
            case GameState.GameOver:
                StopAllCoroutines();
                break;
        }
    }
    
}
