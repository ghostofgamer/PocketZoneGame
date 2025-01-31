using Interfaces;
using UnityEngine;

namespace WeaponContent.Bullets
{
    public class BulletTrigger : MonoBehaviour
    {
        [SerializeField] private int _damage;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                gameObject.SetActive(false);
            }
        }
    }
}
