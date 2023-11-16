using System;
using System.Collections.Generic;
using Data;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Modifiers;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class Weapon : MonoBehaviour
    {
        public bool isActivated = false;
        public WeaponDatasSo weaponDatsSo;
        public Weapons weaponType;
        public Stats characterStats;
        public WeaponStats weaponStats;


        public float timer;
        public float distanceToEnemy;
        public float attackTime;

        public GameObject projectilePrefab;
        public GameObject projectilePoint;

        public CircleCollider2D circleCollider2D;
        public Collider2D colliderHit;

        public List<GameObject> enemiesInRange;
        public bool hasEnemyInRange;
        public GameObject targetEnemy;

        public List<Modifier> modifiers;
        public List<SpecialModifiers> SpecialModifiersList;


        void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            SetSpecialModifiers();
            SetStats();
        }

        void Update()
        {
            timer += Time.deltaTime;

            //transform.right = targetEnemy.transform.position - transform.position;

            if (timer >= weaponStats.attackSpeed)
            {
                if (CanAttack())
                {
                    timer = 0;
                    Attack();   
                }
                else
                {
                    timer = weaponStats.attackSpeed;
                }
            }
        }

        private bool CanAttack()
        {
            //Debug.Log(weaponStats.range);
            return targetEnemy != null && distanceToEnemy <= weaponStats.range/GameConfig.RangeToRadius;
        }

        private void SetSpecialModifiers()
        {
            foreach (var spModifier in SpecialModifiersList)
            {
                modifiers.Add(ModifierCreator.GetModifier(spModifier));
            }
        }

        private void SetStats()
        {
            var data = weaponDatsSo.WeaponData[weaponType];
            weaponStats.damage = data.BaseDamage;
            weaponStats.range = data.BaseAttackRange;
            weaponStats.attackSpeed = data.BaseAttackSpeed;
            weaponStats.projectileAmount = data.BaseProjectileAmount;
            weaponStats.criticalHitChance = data.BaseCriticalHitChance;
            weaponStats.criticalHitDamage = data.BaseCriticalHitDamage;


            if (circleCollider2D != null)
                circleCollider2D.radius = (weaponStats.range / GameConfig.RangeToRadius);
        }

        public void SetEnemy(GameObject enemy)
        {
            if (enemy == null)
            {
                targetEnemy = null;
                return;
            }

            targetEnemy = enemy;
        }
        
        public void SetEnemy(GameObject enemy, float distance)
        {
            if (enemy == null)
            {
                targetEnemy = null;
                return;
            }

            targetEnemy = enemy;

            distanceToEnemy = distance;
            EventManager.Instance.SetDistanceBetweenEnemy(distance*GameConfig.RangeToRadius);
        }

        public void Attack()
        {
            if (weaponType != Weapons.Gun)
                return;
            
            //CheckForEnemy();

            if (targetEnemy == null)
                return;

            //transform.right = targetEnemy.transform.position - transform.position;

            for (int i = 0; i < weaponStats.projectileAmount; i++)
            {
                var projectile = Instantiate(projectilePrefab);
                projectile.transform.position = projectilePoint.transform.position;
                projectile.transform.right = targetEnemy.transform.position - projectile.transform.position;

                var sc = projectile.GetComponent<Projectile>();

                //sc.bounceNum = 1;
                //sc.pierceNum = 1;
                sc.criticalHitChance = weaponStats.criticalHitChance / 100f;
                sc.criticalHitDamage = weaponStats.criticalHitDamage;
                sc.modifiers = modifiers;
                sc.SetMaxDistance(weaponStats.range);
            }
        }

        private void CheckForEnemy()
        {
            for (int i = enemiesInRange.Count - 1; i >= 0; i--)
            {
                if (!enemiesInRange[i].activeSelf)
                {
                    enemiesInRange.Remove(enemiesInRange[i]);
                }
            }

            hasEnemyInRange = enemiesInRange.Count > 0;

            if (hasEnemyInRange)
                targetEnemy = enemiesInRange[0];
            else
                targetEnemy = null;
        }

        private void DealDamage(GameObject enemy)
        {
            var damage = 1;
            var isCriticalHit = Random.Range(0, 1f) <= 0.5f;
            ScriptDictionaryHolder.Enemies[enemy].GetHit(damage, isCriticalHit);
        }

        public void Activate()
        {
            isActivated = true;
            colliderHit.enabled = true;
        }

        public void DeActivate()
        {
            isActivated = false;
            colliderHit.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            //Debug.Log("Entered Range!");
            if (col.CompareTag("Enemy"))
            {
                Debug.Log("Weapon hit enemy!");
                if (isActivated)
                    DealDamage(col.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                Debug.Log("Weapon exit enemy!");
                enemiesInRange.Remove(other.gameObject);
                CheckForEnemy();
            }
        }
    }
}