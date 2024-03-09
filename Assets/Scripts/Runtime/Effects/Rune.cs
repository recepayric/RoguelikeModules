using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UIElements;

namespace Runtime.Effects
{
    public class Rune : MonoBehaviour, IPoolObject
    {
        public PoolKeys PoolKeys { get; set; }
        public bool isRotating;
        public bool isActivated;
        public bool isDetonated;
        public bool isExpanding;
        public bool canExplode;
        public float rotateSpeed;
        public float scaleUpTime;
        public float targetScale;
        public float explosionDelayTime;
        public float explosionDelayTimer;
        public float damage;

        public List<IDamageable> enemiesInRange;

        [Button]
        public void Prepare()
        {
            isExpanding = true;

            if (enemiesInRange == null)
                enemiesInRange = new List<IDamageable>();

            enemiesInRange.Clear();
        }

        public void Explode()
        {
            isActivated = false;
            DealDamageToTargets();
            RemoveObject();
            Debug.Log("Exploded!!");
        }

        public void DealDamageToTargets()
        {
            for (int i = 0; i < enemiesInRange.Count; i++)
            {
                enemiesInRange[i].DealDamage(damage, false, 2);
            }
        }

        private void Update()
        {
            if (isExpanding)
                ScaleUp();

            if (isRotating)
                Rotate();

            if (!isActivated) return;
            explosionDelayTimer -= Time.deltaTime;

            if (explosionDelayTimer <= 0)
            {
                Explode();
            }
        }

        private void Rotate()
        {
            transform.Rotate(new Vector3(0, 0, 1), rotateSpeed);
        }

        private void ScaleUp()
        {
            transform.localScale += (Time.deltaTime * Vector3.one) / scaleUpTime;
            if (transform.localScale.x >= targetScale)
            {
                transform.localScale = Vector3.one * targetScale;
                isExpanding = false;
                canExplode = true;
                CheckIfCanExplode();
            }
        }

        private void CheckIfCanExplode()
        {
            if (enemiesInRange == null)
                enemiesInRange = new List<IDamageable>();

            if (enemiesInRange.Count > 0)
            {
                Activate();
            }
        }

        private void Activate()
        {
            isActivated = true;
            isDetonated = true;
            explosionDelayTimer = explosionDelayTime;
        }

        private void RemoveObject()
        {
            BasicPool.instance.Return(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isDetonated && canExplode)
            {
                Activate();
            }

            var enemy = DictionaryHolder.Damageables[other.gameObject];

            if (enemiesInRange == null)
                enemiesInRange = new List<IDamageable>();

            enemiesInRange.Add(enemy);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!DictionaryHolder.Damageables.ContainsKey(other.gameObject)) return;

            var enemy = DictionaryHolder.Damageables[other.gameObject];

            if (enemiesInRange == null)
                enemiesInRange = new List<IDamageable>();

            if (enemiesInRange.Contains(enemy))
                enemiesInRange.Remove(enemy);
        }

        public void OnGet()
        {
            canExplode = false;
            isExpanding = false;
            isActivated = false;
            isDetonated = false;

            Prepare();
            DictionaryHolder.Runes.Add(gameObject, this);
        }

        public void OnReturn()
        {
            transform.localScale = Vector3.zero;
            DictionaryHolder.Runes.Remove(gameObject);
            enemiesInRange.Clear();
        }
    }
}