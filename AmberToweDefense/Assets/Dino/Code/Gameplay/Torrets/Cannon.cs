using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DINO
{
    public class Cannon : MonoBehaviour
    {
        #region SerializedFields
        
        [Header("Bullet")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float fireRate = 1f;
        [SerializeField] private float bulletSpeed = 1f;
        
        [Header("Cannon")]
        [SerializeField] private SphereCollider cannonCollider;
        [SerializeField] private float visionRadius = 3f;
        [SerializeField] private Transform cannonTransform;
        [SerializeField] private float rotationSpeed = 3f;
        

        
        #endregion

        #region private variables

        private EnemyTarget CurrentTarget
        {
            get => _currentTarget;
            set
            {
                _currentTarget = value;
                Debug.Log("Current target = " + _currentTarget.gameObject.name);
            }
        }
        
        private bool _hasTarget = false;
        private EnemyTarget _currentTarget;
        private CannonState _currentState = CannonState.None;
        Queue<EnemyTarget> _enemiesQueue = new Queue<EnemyTarget>();


        #endregion
        
        
        #region unity methods

        private void Start()
        {
            _currentState = CannonState.Idle;
            cannonCollider.radius = visionRadius;
        }

        private void Update()
        {
            HandleCannonState();
            Debug.DrawRay(cannonTransform.position, cannonTransform.forward * visionRadius, Color.red);
        }

        private void HandleCannonState()
        {
            switch (_currentState)
            {
                case CannonState.None:
                    break;
                case CannonState.Idle:
                    break;
                case CannonState.Shooting:
                    FollowTarget();
                    break;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.GetComponent<EnemyTarget>())
            {
                _hasTarget = true;
                EnemyTarget enemyTarget = other.gameObject.GetComponent<EnemyTarget>();
                OnEnemyEnter(enemyTarget);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.GetComponent<EnemyTarget>())
            {
                OnEnemyExit();
            }
        }

        #endregion
        #region private methods

        
        
        private IEnumerator Shoot(float fireRate)
        {
            while (_hasTarget)
            {
                Fire();
                yield return new WaitForSeconds(fireRate);
            }
        }
        
        private void Fire()
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            CannonBullet cannonBullet = bullet.GetComponent<CannonBullet>();
            cannonBullet.DoParabolicMovement(_currentTarget.transform, 1f, bulletSpeed);
        }

        private void FollowTarget()
        {
            if (_currentTarget == null || _currentTarget.gameObject == null)
            {
                return;
            }
            
            Vector3 targetDirection = _currentTarget.transform.position - cannonTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            cannonTransform.rotation = Quaternion.Slerp(cannonTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        

        #endregion
        
        private void OnEnemyEnter(EnemyTarget enemyTarget)
        {
            _enemiesQueue.Enqueue(enemyTarget);
            _currentState = CannonState.Shooting;

            if (CurrentTarget != null) return;
            
            CurrentTarget = _enemiesQueue.Peek(); 
            StartCoroutine(Shoot(fireRate));
            
        }
        
        private void OnEnemyExit()
        {
            _enemiesQueue.Dequeue();

            if (_enemiesQueue.Count <= 0)
            {
                _hasTarget = false;
                _currentState = CannonState.Idle;
                StopCoroutine(Shoot(fireRate));
                return;
            }
            CurrentTarget = _enemiesQueue.Peek();
        }
        
    }
    
    public enum CannonState
    {
        None,
        Idle,
        Shooting
    }
}