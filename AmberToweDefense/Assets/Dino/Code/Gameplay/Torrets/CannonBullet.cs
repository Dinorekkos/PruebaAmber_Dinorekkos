using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private float bulletLifeTime = 3f;
     private int _damage = 5;
    #endregion

    #region public variables

    public Transform Target
    {
        get => _target;
    }

    #endregion
    #region private variables
    
    private Transform _target;
    
    #endregion

    #region unity methods
    
    void Start()
    {
        
    }

    void Update()
    {
        bulletLifeTime -= Time.deltaTime;
        if (bulletLifeTime <= 0)
        {
            Despawn();
        }
    }
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyTarget>())
        {
            other.GetComponent<EnemyTarget>().ReceiveDamage(5);
            Despawn();
        }
    }
    #endregion


    #region public Methods

    public void UpdateTarget(Transform target)
    {
        _target = target;
    }
    public void DoParabolicMovement(Transform target, float height, float duration, int damage)
    {
       _damage = damage;
        StartCoroutine(ParabolicMovement(target, height, duration));
        
    }
    #endregion

    #region private methods
    
    IEnumerator ParabolicMovement(Transform target, float height, float duration)
    {
        Vector3 startPosition = transform.position;
        
        float time = 0f;
        while (time < duration)
        {
            _target = target;
            Vector3 endPosition = _target.position;
            float t = time / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * height;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = _target.position;
    }

    
    private void Despawn()
    {
        Destroy(gameObject);
    }

    #endregion

}
