using System;
using DG.Tweening;
using Runtime.Enums;
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

        public bool ExplodeContinuous = false;
        public float explosionTime;
        private float explosionTimer;

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

        [Button]
        public void Explode()
        {
            Debug.Log("Position: " + transform.position);
            transform.localScale = Vector3.one * explosionRange * 2;

            var numTargetsInRange =
                Physics2D.OverlapCircleNonAlloc(transform.position, explosionRange, collider2Ds); //layermask to filter the varius useless colliders
            Debug.Log("explosion affected: " + numTargetsInRange);


            for (int i = 0; i < numTargetsInRange; i++)
            {
                if (collider2Ds[i] == null) break;

                if (collider2Ds[i].CompareTag("Enemy"))
                {
                    DealDamage(collider2Ds[i].gameObject);
                }
            }
        }

        private void DealDamage(GameObject enemy)
        {
            enemy.GetComponent<Enemy>().DealDamage(1, false);
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnGet()
        {
            ScriptDictionaryHolder.Explosions.Add(gameObject, this);
            SetStats();
            DOVirtual.DelayedCall(1, () => BasicPool.instance.Return(gameObject));
        }

        public void OnReturn()
        {
            ScriptDictionaryHolder.Explosions.Remove(gameObject);
        }
    }
}