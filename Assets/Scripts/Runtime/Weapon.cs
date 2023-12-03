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
        
        public static float rotationGlobal;
        
        
        void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            SetStats();
            SetSpecialModifiers();
            ActivateModifiers();
        }

        void Update()
        {
            rotationGlobal += Time.deltaTime*100f;
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
            
            foreach (var spModifier in weaponStats.specialModifiers)
            {
                modifiers.Add(ModifierCreator.GetModifier(spModifier));
            }
        }

        private void ActivateModifiers()
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].ApplyEffect(this);
            }
        }

        private void SetStats()
        {
            //extra damages
            var extraDamage = 0f;

            if (weaponType == Weapons.Gun)
                extraDamage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.RangedAttack);
           
            if (weaponType == Weapons.Sword)
                extraDamage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.MeleeAttack);

            
            weaponStats.specialModifiers = weaponDatsSo.specialModifiersList;
            
            var data = weaponDatsSo.WeaponData[weaponType];
            weaponStats.damage = data.BaseDamage + extraDamage;
            weaponStats.range = data.BaseAttackRange;
            weaponStats.attackSpeed = data.BaseAttackSpeed;
            weaponStats.projectileAmount = data.BaseProjectileAmount;
            weaponStats.criticalHitChance = data.BaseCriticalHitChance;
            weaponStats.criticalHitDamage = data.BaseCriticalHitDamage;

            weaponStats.pierceNum = (int)(data.BasePierceNumber+ScriptDictionaryHolder.Player.stats.GetStat(AllStats.PierceNumber));
            
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
                //todo change this to pool and dictionary.
                var projectile = Instantiate(projectilePrefab);
                projectile.transform.position = projectilePoint.transform.position;
                projectile.transform.right = targetEnemy.transform.position - projectile.transform.position;

                var sc = projectile.GetComponent<Projectile>();

                //sc.bounceNum = 1;
                sc.pierceNum = weaponStats.pierceNum;
                sc.criticalHitChance = weaponStats.criticalHitChance / 100f;
                sc.criticalHitDamage = weaponStats.criticalHitDamage;
                sc.weapon = this;
                sc.SetModifiers(modifiers);
                sc.SetMaxDistance(weaponStats.range);
                sc.SetHomingProjectile(true, targetEnemy);
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
                if (isActivated)
                    DealDamage(col.gameObject);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Enemy"))
            {
                enemiesInRange.Remove(other.gameObject);
                CheckForEnemy();
            }
        }
    }
}