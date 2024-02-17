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
                _currentEnemyDirection += _pathManager.GetRotationWaypoint(value);
                enemyTransform.DORotate(new Vector3(0, _currentEnemyDirection, 0), 0.7f);
            }
        }
        
        private int _currentWaypoint = 0;
        private float _currentEnemyDirection = 0f;
        private PathManager _pathManager;
        
        private Action OnWaypointChange;
        #endregion

        #region unity methods
        void Start()
        {
            _pathManager = PathManager.Instance;
            _currentEnemyDirection = enemyTransform.localRotation.eulerAngles.y;
            DoMovement();
        }

        private void FixedUpdate()
        {
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
                Debug.Log("Waypoint changed = ".SetColor("#C7E21A") + _currentWaypoint + " =  " + _pathManager.GetRotationWaypoint(_currentWaypoint));
                
                _currentWaypoint++;
               
                
                Debug.Log("my rotation is ".SetColor("#1AD3E2") + _currentEnemyDirection);

            }
            
            
            // transform.DOLocalPath(_pathManager.Path, velocity).SetEase(Ease.Linear)
            //     .OnWaypointChange((i) => { Rotate = i; });
        }
        #endregion
        
        

    }
}