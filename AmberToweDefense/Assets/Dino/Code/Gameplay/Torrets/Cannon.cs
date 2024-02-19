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
        
        [Header("Cannon Level")] 
        [SerializeField] private CannonData cannonData;

        [Header("Bullet")] 
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform bulletSpawnPoint;
        [SerializeField] private float fireRate = 1f;

        [Header("Cannon")] 
        [SerializeField] private SphereCollider cannonCollider;
        [SerializeField] private Transform cannonTransform;
        [SerializeField] private float rotationSpeed = 3f;

        [Header("Currency")] 
        [SerializeField] private int buildCost = 5;

        [Header("UI")] 
        [SerializeField] private GameObject upgradeButton;

        #endregion

        public Action<CannonState> OnCannonStateChange;


        #region private variables

        private CannonLevel _canonLevel;
        
        private int _currentLevel = 1;    
        private int _upgradeCost = 5;


        private EnemyTarget CurrentTarget
        {
            get => _currentTarget;
            set
            {
                _currentTarget = value;
                // Debug.Log("Current target = " + _currentTarget.gameObject.name);
            }
        }
        
        private CannonState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;
                OnCannonStateChange?.Invoke(_currentState);
            }
        }
        
        private bool _hasTarget = false;
        private EnemyTarget _currentTarget;
        private CannonState _currentState = CannonState.None;
        Queue<EnemyTarget> _enemiesQueue = new Queue<EnemyTarget>();
        private Coroutine _shootingCoroutine;

        #endregion
        
        
        #region unity methods

        private void Start()
        {
            _currentLevel = 1;
            UpdateLevelFromMultiplier();
            CurrentState = CannonState.UnBuild;
            cannonTransform.gameObject.SetActive(false);
            GameplayController.Instance.OnGameStateChanged += HandleGameStateChanged;
            CurrencyManager.Instance.OnCurrencyChanged += CheckUpgradeButton;
        }
        
        private void Update()
        {
            if (_hasTarget)
            {
                FollowTarget();
                Debug.DrawRay(cannonTransform.position, cannonTransform.forward * _canonLevel.Range, Color.red);
            }
            
            if (CurrentTarget != null && CurrentTarget.IsDead())
            {
                OnEnemyDie(CurrentTarget);
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

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.GetComponent<EnemyTarget>())
            {
                EnemyTarget enemyTarget = other.gameObject.GetComponent<EnemyTarget>();
                if (_enemiesQueue.Contains(enemyTarget) == false)
                {
                    OnEnemyEnter(enemyTarget);
                }
            }
        }

        private void OnDestroy()
        {
            // GameplayController.Instance.OnGameStateChanged -= HandleGameStateChanged;
        }

        #endregion
        
        #region public methods
        public void HandleClickCannon()
        {
            HandleCannonState();
        }
        #endregion
        #region private methods
        
        private void UpdateLevelFromMultiplier()
        {
            if(_currentLevel > cannonData.CannonLevels.Count) return;
            _canonLevel = cannonData.CannonLevels.Find((x) => _currentLevel == x.Level);
            cannonCollider.radius = _canonLevel.Range;
            _upgradeCost = _canonLevel.UpgradeCost;
        }
        private void HandleGameStateChanged(GameState gameState)
        {
            if (gameState == GameState.GameOver)
            {
                ResetCannonState();
            }
        }
        private void HandleCannonState()
        {
            switch (_currentState)
            {
                case CannonState.Idle:
                case CannonState.Shooting:
                    UpgradeCannon();
                    break;
                
                case CannonState.UnBuild:
                    BuildCannon();
                    break;
               
                
            }
        }

        private void BuildCannon()
        {
            if (CurrencyManager.Instance.CanAfford(buildCost) == false) return;
            CurrentState = CannonState.Idle;
            cannonTransform.gameObject.SetActive(true);
            CurrencyManager.Instance.SpendCurrency(buildCost);
        }

        private void UpgradeCannon()
        {
            if (CurrencyManager.Instance.CanAfford(_upgradeCost) == false) return;
            
            CurrencyManager.Instance.SpendCurrency(_upgradeCost);
            _currentLevel++;
            UpdateLevelFromMultiplier();
            

        }
        
        private void CheckUpgradeButton(int currency = 0)
        {
            if(_currentState == CannonState.UnBuild) return;
            if (CurrencyManager.Instance.CanAfford(_upgradeCost))
            {
                upgradeButton.SetActive(true);
            }
            else
            {
                upgradeButton.SetActive(false);
            }
        }
        
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
            
            cannonBullet.DoParabolicMovement(_currentTarget.transform, 1f, _canonLevel.speedCannon, _canonLevel.Damage);
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
            if(_currentState == CannonState.UnBuild) return;
            
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
                CurrentState = CannonState.Shooting;
            }
        }
        
        private void OnEnemyExit()
        {
            if(_currentState == CannonState.UnBuild || _enemiesQueue.Count == 0) 
            {
                ResetCannonState();
                return;
            }
            _enemiesQueue.Dequeue();
            
            if (_enemiesQueue.Count == 0) ResetCannonState();
            else CurrentTarget = _enemiesQueue.Peek();

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
            CurrentState = CannonState.Idle;
            StopCoroutine(Shoot(fireRate));
        }
    }
    
    public enum CannonState
    {
        None,
        UnBuild,
        Idle,
        Shooting
    }
}