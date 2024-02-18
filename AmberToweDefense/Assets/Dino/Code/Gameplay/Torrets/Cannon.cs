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
                // Debug.Log("Current target = " + _currentTarget.gameObject.name);
            }
        }
        
        private bool _hasTarget = false;
        private bool _isShooting = false;
        private EnemyTarget _currentTarget;
        private CannonState _currentState = CannonState.None;
        Queue<EnemyTarget> _enemiesQueue = new Queue<EnemyTarget>();
        private Coroutine _shootingCoroutine;

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
            
            if (CurrentTarget != null && CurrentTarget.IsDead())
            {
                Debug.Log("Enemy is dead");
                OnEnemyDie(CurrentTarget);
                
            }
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

            if (CurrentTarget == null || CurrentTarget.gameObject == null) return;
            
            cannonBullet.DoParabolicMovement(_currentTarget.transform, 1f, bulletSpeed);
        }
        private void FollowTarget()
        {
            if (_currentTarget == null || _currentTarget.gameObject == null) return;
            
            Vector3 targetDirection = _currentTarget.transform.position - cannonTransform.position;
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            cannonTransform.rotation = Quaternion.Slerp(cannonTransform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        

        #endregion
        
        private void OnEnemyEnter(EnemyTarget enemyTarget)
        {
            _hasTarget = true;
            _enemiesQueue.Enqueue(enemyTarget);
            if (_currentState != CannonState.Shooting)
            {
                CurrentTarget = _enemiesQueue.Peek();
                if (_shootingCoroutine != null)
                {
                    StopCoroutine(_shootingCoroutine);
                }
                _shootingCoroutine = StartCoroutine(Shoot(fireRate));
                _currentState = CannonState.Shooting;
            }
        }
        
        private void OnEnemyExit()
        {
            _enemiesQueue.Dequeue();
            if (_enemiesQueue.Count == 0)
            {
                _hasTarget = false;
                _currentState = CannonState.Idle;
                StopCoroutine(Shoot(fireRate));
                return;
            }
            CurrentTarget = _enemiesQueue.Peek();
            // OnEnemyDie(existingEnemy);
        }

        private void OnEnemyDie(EnemyTarget deadEnemy)
        {
            DestroyBulletsTargetingEnemy(deadEnemy);
            UpdateCurrentTargetAfterEnemyDeath(deadEnemy);
        }
        
        private void DestroyBulletsTargetingEnemy(EnemyTarget deadEnemy)
        {
            CannonBullet[] bullets = FindObjectsOfType<CannonBullet>();

            foreach (CannonBullet bullet in bullets)
            {
                if (bullet.Target == deadEnemy.transform)
                {
                    // Debug.Log("Target Die".SetColor("#FE6644"));
                    Destroy(bullet.gameObject);
                }
            }
        }
        private void UpdateCurrentTargetAfterEnemyDeath(EnemyTarget deadEnemy)
        {
            if (deadEnemy == CurrentTarget && _enemiesQueue.Count > 0)
            {
                _enemiesQueue.Dequeue();
                if (_enemiesQueue.Count > 0)
                {
                    CurrentTarget = _enemiesQueue.Peek();
                }
                else
                {
                    ResetCannonState();
                }
            }
        }
        
        private void ResetCannonState()
        {
            _hasTarget = false;
            _currentState = CannonState.Idle;
            StopCoroutine(Shoot(fireRate));
        }
    }
    
    public enum CannonState
    {
        None,
        Idle,
        Shooting
    }
}