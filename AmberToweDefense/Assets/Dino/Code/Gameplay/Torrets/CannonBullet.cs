using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBullet : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] private float bulletLifeTime = 3f;
    #endregion
    
    #region private variables
    
    
    #endregion
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
    

    #region public variable

    public void DoParabolicMovement(Transform target, float height, float duration)
    {
        StartCoroutine(ParabolicMovement(target, height, duration));
        
    }
    
    IEnumerator ParabolicMovement(Transform target, float height, float duration)
    {
        Vector3 startPosition = transform.position;
        
        float time = 0f;
        while (time < duration)
        {
            Vector3 endPosition = target.position;
            float t = time / duration;
            transform.position = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * Mathf.Sin(t * Mathf.PI) * height;
            time += Time.deltaTime;
            yield return null;
        }
        transform.position = target.position;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyTarget>())
        {
            other.GetComponent<EnemyTarget>().ReceiveDamage(10);
            Despawn();
        }
    }
    
    private void Despawn()
    {
        Destroy(gameObject);
    }

    #endregion
}
