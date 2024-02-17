using System;
using System.Collections;
using System.Collections.Generic;
using com.LazyGames;
using UnityEngine;

namespace com.LazyGames
{
    public class TimerBase : MonoBehaviour
    {
        #region public variables
        public float CurrentTimer => _currentTimer;
        public Action OnTimerStart;
        public Action OnTimerEnd;
        public Action<float> OnTimerUpdate;
        public Action OnTimerLoop;
        public bool IsTimerActive => _isTimerActive; 
        #endregion

        #region private variables
        private bool _isTimerActive;
        private bool _isLoopable;
        private float _currentTimer;
        private float _loopTimer;

        private float _updateInterval = 1f; 
        private float _elapsedTime;

        private bool _isCountdown;
        #endregion
        
        #region unity methods
        void Update()
        {
            if (_isTimerActive)
            {
                if(_isCountdown) _currentTimer -= 1 * Time.deltaTime;
                else _currentTimer += 1 * Time.deltaTime;
                
                if (_currentTimer <= 0.9f)
                {
                    if(_isLoopable)
                    {
                        _currentTimer = _loopTimer;
                        OnTimerLoop?.Invoke();
                    }
                    else
                        FinishedTimer();
                }
            
                _elapsedTime += Time.deltaTime;
                if (_elapsedTime >= _updateInterval)
                {
                    OnTimerUpdate?.Invoke(_currentTimer);
                    _elapsedTime = 0.0f;
                }
                
            }
        }
        #endregion

        #region public methods

        public void StartTimer(float timer, bool isCountDown = false, float intervalUpdate = 1f,string message = "")
        {
            _currentTimer = timer;
            _updateInterval = intervalUpdate;
            _isCountdown = isCountDown;
            
            _isTimerActive = true;
            OnTimerStart?.Invoke();
            
            if(!string.IsNullOrEmpty(message))
             Debug.Log("Timer Started = " + message + " Timer = " + _currentTimer);
        }

        public void SetLoopableTimer(float timer, bool isCountDown = false, float intervalUpdate = 1f,string message = "")
        {
            _currentTimer = timer;
            _updateInterval = intervalUpdate;
            _loopTimer = timer;
            _isCountdown = isCountDown;
            
            _isTimerActive = true;
            _isLoopable = true;
            
            if(!string.IsNullOrEmpty(message))
                Debug.Log("Timer Started = " + message + " Timer = " + _currentTimer);
        }
        public void PauseTimer()
        {
            _isTimerActive = false;
        }

        public void ResetTimer()
        {
            _currentTimer = 0;
        }
        
        #endregion

        #region private methods

        private void FinishedTimer()
        {
            _isTimerActive = false;
            ResetTimer();
            OnTimerEnd?.Invoke();
        }

        #endregion
    }

}