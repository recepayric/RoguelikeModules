using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class Health : MonoBehaviour
    {
        public float maxHealth;
        public float currentHealth;

        public float healthPercentage;

        public GameObject healthBar;

        public void SetMaxHealth(float _maxHealth)
        {
            maxHealth = _maxHealth;
            currentHealth = maxHealth;
        }

        public void UpdateMaxHealth(float _maxHealth)
        {
            maxHealth = _maxHealth;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
        }
        public void UpdateHealth(float _currentHealth)
        {
            currentHealth = _currentHealth;
            UpdateHealthBar();
        }

        public void UpdateHealthBar()
        {
            return;
            healthPercentage = currentHealth / maxHealth;
            healthBar.transform.localScale = new Vector3(healthPercentage, 1, 1);
        }
    }
}