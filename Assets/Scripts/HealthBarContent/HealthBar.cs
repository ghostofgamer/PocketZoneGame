using UnityEngine;
using UnityEngine.UI;

namespace HealthBarContent
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Image _healthImage;
        
        public void SetHealth(float currentHealth, float maxHealth)
        {
            _healthImage.fillAmount = currentHealth / maxHealth;
        }
    }
}