using System;
using UnityEngine.Events;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class HealthVO
    {
        public event UnityAction HealthUpdatedEvent;
        public event UnityAction HealthFinishedEvent;
        public event UnityAction StopHealthRegenEvent;
        public event UnityAction StartHealthRegenEvent;
        public event UnityAction DamageTakenEvent;
        
        public float damageTaken;
        public float currentHealth;
        public float maxHealth;
        public float healthRegen;

        public void DealDamage(float change)
        {
            damageTaken += change;
            
            if (damageTaken > maxHealth) damageTaken = maxHealth;

            currentHealth = maxHealth - damageTaken;
            DamageTakenEvent?.Invoke();

            if (currentHealth <= 0)
            {
                HealthFinishedEvent?.Invoke();
            }
        }

        public void HealthUpdated()
        {
            HealthUpdatedEvent?.Invoke();
        }

        public void RegenHealth(float increaseAmount)
        {
            damageTaken -= increaseAmount;
            if (damageTaken < 0) damageTaken = 0;
            currentHealth = maxHealth - damageTaken;
            //HealthUpdatedEvent?.Invoke();
        }

        public void StartHealthRegen()
        {
            StartHealthRegenEvent?.Invoke();
        }
        
        public void StopHealthRegen()
        {
            StopHealthRegenEvent?.Invoke();
        }
    }
}