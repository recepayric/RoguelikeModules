using System;
using System.Collections.Generic;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class Projectile : MonoBehaviour, IPoolObject
    {
        public bool destroyAfterTravelingMax = true;
        public bool followTarget = false;

        public int bounceNum;
        public int pierceNum;

        public float criticalHitChance;
        public float criticalHitDamage;

        public float projectileSpeed = 3;
        public Transform pTransform;
        public float distanceTraveled = 0;
        public Vector3 deltaTravel;

        public float maxTravel = 200;

        public Collider2D[] collider2Ds;

        public List<Modifier> modifiers;

        public bool isStickingToEnemy = false;
        public bool doesExplode = false;
        public float explosionTimer;

        //Split
        public bool split;
        public int splitAmount;
        public GameObject ignoredEnemy;
        public PoolKeys splitProjectileKey;

        private void Start()
        {
            collider2Ds = new Collider2D[100];
            distanceTraveled = 0;
            //maxTravel = 200f / GameConfig.RangeToRadius;
            pTransform = transform;
            //Debug.Log("Projectile created!");
        }

        public void SetMaxDistance(float weaponDistance)
        {
            maxTravel = weaponDistance / GameConfig.RangeToRadius;
        }

        private void ResetTravelData()
        {
            distanceTraveled = 0;
        }

        private void StopTravelDestroy()
        {
            destroyAfterTravelingMax = false;
        }

        private void StartTravelDestroy()
        {
            ResetTravelData();
            destroyAfterTravelingMax = true;
        }

        private void Update()
        {
            deltaTravel = Time.deltaTime * projectileSpeed * pTransform.right;
            distanceTraveled += Vector2.Distance(pTransform.position, pTransform.position + deltaTravel);
            pTransform.position += deltaTravel;

            if (destroyAfterTravelingMax && distanceTraveled >= maxTravel)
            {
                Debug.Log("Travelled: " + distanceTraveled + "    " + maxTravel);
                Destroy(gameObject);
            }
        }

        public void HitTarget(Enemy enemy)
        {
            if (enemy.gameObject == ignoredEnemy)
                return;

            //ResetTravelData();
            var isCrit = Random.Range(0, 1f) <= criticalHitChance;

            enemy.GetHit(1, isCrit);

            foreach (var modifier in modifiers)
            {
                modifier.ApplyEffect(gameObject, this, isCrit);
            }

            if (bounceNum > 0)
            {
                StopTravelDestroy();
                ResetTravelData();
                bounceNum--;
                var newTarget = GetClosestEnemy(enemy.gameObject);
                if (newTarget == null)
                    Destroy(gameObject);
                else
                    transform.right = newTarget.transform.position - transform.position;
            }
            else if (pierceNum > 0)
            {
                pierceNum--;
                StartTravelDestroy();
            }
            else if (split)
            {
                CommandController.instance.SplitProjectile(enemy.transform.position, splitAmount, splitProjectileKey, enemy.gameObject);
                Destroy(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private GameObject GetClosestEnemy(GameObject currentEnemy)
        {
            Physics2D.OverlapCircleNonAlloc(transform.position, 500, collider2Ds); //layermask to filter the varius useless colliders

            foreach (var collidedEnemy in collider2Ds)
            {
                if (collidedEnemy == null) break;

                if (collidedEnemy.CompareTag("Enemy") && collidedEnemy.gameObject != currentEnemy)
                {
                    return collidedEnemy.gameObject;
                }
            }


            return null;
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
            ScriptDictionaryHolder.Projectiles.Remove(gameObject);
        }

        public void OnGet()
        {
            distanceTraveled = 0;
            ScriptDictionaryHolder.Projectiles.Add(gameObject, this);
        }
    }
}