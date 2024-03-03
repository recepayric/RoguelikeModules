using System;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Effects
{
    public class Explosion : MonoBehaviour, IPoolObject
    {
        public Collider2D[] collider2Ds;

        public float explosionRange;
        public float explosionDamage;
        public float criticalHitChance;
        public float criticalHitDamage;
        public LayerMask layerMask;

        public bool ExplodeContinuous = false;
        public float explosionTime;
        private float explosionTimer;

        public ParticleSystem ParticleSystem;

        // Start is called before the first frame update
        void Start()
        {
        }

        private void Awake()
        {
            collider2Ds = new Collider2D[50];
        }

        private void SetStats()
        {
            //explosionRange = 1;
            explosionDamage = 1;
        }

        public void SetDamage(float damage)
        {
            explosionDamage = damage;
        }
        

        // Update is called once per frame
        void Update()
        {
            explosionTimer += ExplodeContinuous ? Time.deltaTime : 0;
            if (explosionTimer >= explosionTime)
            {
                Explode();
                explosionTimer -= explosionTimer;
            }
        }

        public void SetSize(float size)
        {
            explosionRange = size;
        }

        public void SetRange(float range)
        {
            var rangeConv = range;
            Debug.Log("range: " + range);

            explosionRange = rangeConv*2;
        }
        
        [Button]
        public void Explode()
        {
            ParticleSystem.Play();
            transform.localScale = Vector3.one * explosionRange;

            var numTargetsInRange =
                Physics2D.OverlapCircleNonAlloc(transform.position, explosionRange/2, collider2Ds, layerMask); //layermask to filter the varius useless colliders
            //Debug.Log("explosion affected: " + numTargetsInRange);


            for (int i = 0; i < numTargetsInRange; i++)
            {
                if (collider2Ds[i] == null) break;
                var damageable = DictionaryHolder.Damageables[collider2Ds[i].gameObject];
                if (damageable != null)
                {
                    DealDamage(damageable);
                }
            }
        }

        private void DealDamage(IDamageable damageable)
        {
            Debug.Log("Dealing damage to: " + damageable);
            damageable.DealDamage(explosionDamage, false, 1);
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnGet()
        {
            DictionaryHolder.Explosions.Add(gameObject, this);
            SetStats();
            DOVirtual.DelayedCall(explosionTime*1.1f, () => BasicPool.instance.Return(gameObject));
        }

        public void OnReturn()
        {
            DictionaryHolder.Explosions.Remove(gameObject);
        }
    }
}