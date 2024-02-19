using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DINO
{
    public class GameplayController : MonoBehaviour
    {
        
        #region public variables

        public static GameplayController Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<GameplayController>();
                }

                return _instance;
            }
        }

        public GameState CurrentGameState
        {
            get => _currentGameState;
            set
            {
                _currentGameState = value;
                HandleGameStates();
                OnGameStateChanged?.Invoke(_currentGameState);
            }
        }
        
        public Action<GameState> OnGameStateChanged;
        public Action OnGameOver;
        #endregion

        #region private variables
        
        private static GameplayController _instance;
        private GameState _currentGameState = GameState.None;
        
        #endregion

        void Awake()
        {
            _instance = this;
        }

       

        private void Update()
        {
            // if (Input.GetKeyDown(KeyCode.Space))
            // {
            //     CurrentGameState = GameState.Playing;
            // }
        
        }
        
        public void CheckEnemiesRemaining()
        {
            EnemyTarget[] enemies = FindObjectsOfType<EnemyTarget>();

            if (AllEnemiesAreDead(enemies) && EnemySpawner.Instance.CurrentWave == EnemySpawner.Instance.MaxWave)
            {
                ChangeGameState(GameState.GameOver);
            }
            else if (AllEnemiesAreDead(enemies) && EnemySpawner.Instance.CurrentWave < EnemySpawner.Instance.MaxWave)
            {
                ChangeGameState(GameState.Preparing);
            }
          
                
        }
        
        public void ChangeGameState(GameState gameState)
        {
            if (gameState == CurrentGameState) return;
            CurrentGameState = gameState;
        }
        
        private bool AllEnemiesAreDead(EnemyTarget[] enemies)
        {
            foreach (var enemy in enemies)
            {
                if (!enemy.IsDead())
                {
                    return false;
                }
            }

            return true;
        }
        private void HandleGameStates()
        {
            switch (CurrentGameState)
            {
                case GameState.None:
                    break;
               
                case GameState.GameOver:
                    OnGameOver?.Invoke();
                    break;
              
            }
        }
       
    }
    
    public enum GameState
    {
        None,
        Preparing,
        Playing,
        GameOver
    }
}