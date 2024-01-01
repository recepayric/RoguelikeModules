using System;
using Runtime.Enums;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.EnemyRelated.EnemyExtentions
{
    public class WindShield : MonoBehaviour, IPoolObject
    {
        public float damage;
        public float knockBackAmount;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Hit something!! " + other.name);
            //todo get player damagable from dictionary later!!!!
            var damagable = other.GetComponent<IDamageable>();
            
            if (damagable == null) return;
            
            damagable.DealDamage(damage, false);
        }

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
        }

        public void OnGet()
        {
        }
    }
}