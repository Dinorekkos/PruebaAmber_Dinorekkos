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
                OnGameStateChanged?.Invoke(_currentGameState);
            }
        }
        
        public Action<GameState> OnGameStateChanged;
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
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Debug.Log("Space pressed");
                CurrentGameState = GameState.Playing;
            }
        
        }
    }
    
    public enum GameState
    {
        None,
        Countdown,
        Playing,
        GameOver
    }
}