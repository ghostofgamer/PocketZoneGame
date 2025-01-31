using HealthBarContent;
using Interfaces;
using UnityEngine;

namespace EnemyContent
{
    [RequireComponent(typeof(Enemy))]
    public class EnemyHealth : MonoBehaviour,IDamageable
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private int _maxHealth = 100;
    
        private int _currentHealth;
        private Enemy _enemy;

        private void Start()
        {
            _enemy = GetComponent<Enemy>();
            _currentHealth = _maxHealth;
            _healthBar.SetHealth(_currentHealth, _maxHealth);
        }
    
        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            
            _currentHealth -= damage;

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }
            
            _healthBar.SetHealth(_currentHealth, _maxHealth);
        }

        private void Die()
        {
            Instantiate(_enemy.ItemPickUp, transform.position, Quaternion.identity);
            gameObject.SetActive(false);
        }
    }
}