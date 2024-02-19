using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace DINO
{
    public class EnemyMovement : MonoBehaviour
    {
        #region SerializedFields
        
        [SerializeField] private float velocity = 10f;
        [SerializeField] private Transform enemyTransform;
        
        
        #endregion

        #region private variables
        private int Rotate
        {
            set
            {
                _currentEnemyDirection = enemyTransform.localRotation.eulerAngles.y;
                _currentEnemyDirection = _pathManager.GetRotationWaypoint(value);
                StartCoroutine(RotateOverTime(enemyTransform, new Vector3(0, _currentEnemyDirection, 0), 0.2f));
            }
        }
        
        private int _currentWaypoint = 0;
        private float _currentEnemyDirection = 0f;
        private PathManager _pathManager;
        private GameplayController _gameplayController;
        
        #endregion

        #region unity methods
        void Start()
        {
            _pathManager = PathManager.Instance;
            _gameplayController = GameplayController.Instance;
            _currentEnemyDirection = enemyTransform.localRotation.eulerAngles.y;
            DoMovement();
        }

        private void FixedUpdate()
        {
            if(_gameplayController.CurrentGameState == GameState.GameOver)
            {
                return;
            }
                
            DoMovement();
        }

        #endregion
        
        #region private methods
        
        private void DoMovement()
        {
            if (_currentWaypoint == _pathManager.Path.Length)
            {
                _currentWaypoint = 0;
                gameObject.SetActive(false);
                return;
            }
            transform.position = Vector3.MoveTowards(transform.position, _pathManager.Path[_currentWaypoint], velocity * Time.deltaTime);

            if (transform.position == _pathManager.Path[_currentWaypoint])
            {
                Rotate = _currentWaypoint;
                _currentWaypoint++;
            }
            
        }
        
        IEnumerator RotateOverTime(Transform transformToRotate, Vector3 targetRotation, float duration)
        {
            Quaternion startRotation = transformToRotate.rotation;
            Quaternion endRotation = Quaternion.Euler(targetRotation);
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float normalizedTime = elapsed / duration;
                transformToRotate.rotation = Quaternion.Slerp(startRotation, endRotation, normalizedTime);
                yield return null;
            }

            transformToRotate.rotation = endRotation;
        }
        #endregion
        
        

    }
}