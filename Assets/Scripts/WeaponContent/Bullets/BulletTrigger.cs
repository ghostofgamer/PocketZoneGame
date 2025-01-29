using System;
using Interfaces;
using UnityEngine;

public class BulletTrigger : MonoBehaviour
{
    [SerializeField] private int _damage;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Damage triggered");
            damageable.TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }

    /*private void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.gameObject.name);
        
        if (other.gameObject.TryGetComponent(out IDamageable damageable))
        {
            Debug.Log("Damage triggered");
            damageable.TakeDamage(_damage);
            gameObject.SetActive(false);
        }
    }*/
}
