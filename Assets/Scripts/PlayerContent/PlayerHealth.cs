using System;
using HealthBarContent;
using Interfaces;
using SaveDataContent;
using UnityEngine;

namespace PlayerContent
{
    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private HealthBar _healthBar;
        [SerializeField] private int _maxHealth = 100;
        [SerializeField] private Storage _storage;
        [SerializeField] private Inventory _inventory;

        private int _currentHealth;

        public event Action HealthChanged;

        public int Health => _currentHealth;

        private void Start()
        {
            SaveData saveData = _storage.LoadDataInfo();

            if (saveData != null)
            {
                _currentHealth = saveData.health;

                if (saveData.health == 0)
                    _currentHealth = _maxHealth;
            }
            else
            {
                _currentHealth = _maxHealth;
            }

            _healthBar.SetHealth(_currentHealth, _maxHealth);
            _storage.SavePlayerHealth();
        }

        public void TakeDamage(int damage)
        {
            if (damage <= 0) return;
            
            _currentHealth -= damage;
            HealthChanged?.Invoke();

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                Die();
            }

            _healthBar.SetHealth(_currentHealth, _maxHealth);
            _storage.SavePlayerHealth();
        }

        public void Healing()
        {
            _currentHealth = _maxHealth;
            HealthChanged?.Invoke();
            _healthBar.SetHealth(_currentHealth, _maxHealth);
            _storage.SavePlayerHealth();
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}