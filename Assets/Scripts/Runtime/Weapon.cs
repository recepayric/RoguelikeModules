using System;
using System.Collections.Generic;
using Data;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.Modifiers;
using Runtime.ParticleShaderScripts;
using Runtime.WeaponRelated;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(WeaponLevelSystem))]
    [RequireComponent(typeof(WeaponUpgradeTree))]
    public class Weapon : MonoBehaviour
    {
        public WeaponLevelSystem weaponLevelSystem;
        public WeaponUpgradeTree weaponUpgradeTree;
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

        public List<SpecialModifiers> specialModifiersList;
        public List<Modifier> modifiers;
        public List<Modifier> modifiersOnStart;
        public List<Modifier> modifiersOnGetHit;
        public List<Modifier> modifiersOnHealthChange;
        public List<Modifier> modifiersOnItemBuy;

        public static float rotationGlobal;

        public SwirlObject swirlObject;

        public bool isAttacking = false;


        void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            SetStats();
            //SetSpecialModifiers();
            SetSpecialModifiers(specialModifiersList);
            SetSpecialModifiers(weaponStats.specialModifiers);
            ActivateModifiers();
        }

        void Update()
        {
            rotationGlobal += Time.deltaTime * 100f;
            //timer += Time.deltaTime;

            //transform.right = targetEnemy.transform.position - transform.position;
            if (CanAttack())
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    EventManager.Instance.LiftWand(weaponStats.attackSpeed);
                    swirlObject.StartSwirling(weaponStats.attackSpeed);
                }
                timer += Time.deltaTime;
                if (timer >= weaponStats.attackSpeed)
                {
                    Attack();
                    timer = 0;
                }
            }
            else
            {
                if (isAttacking)
                {
                    swirlObject.StopSwirling();
                    isAttacking = false;
                    EventManager.Instance.DownWand();
                }
                timer = 0;
            }

            swirlObject.transform.position = projectilePoint.transform.position;

            /*if (timer >= weaponStats.attackSpeed)
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
            }*/
        }

        public void LiftUpWand()
        {
            
        }

        public void UpdateAttackSpeed()
        {
            var attackSpeedBuff = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.AttackSpeed);
            var currentSpeed = weaponDatsSo.WeaponData[weaponType].BaseAttackSpeed;
            var multiplier = (1 + attackSpeedBuff / 100f);
            if (multiplier == 0)
                multiplier = 0.01f;
            weaponStats.attackSpeed = currentSpeed / multiplier;
        }

        private bool CanAttack()
        {
            //Debug.Log(weaponStats.range);
            return targetEnemy != null && distanceToEnemy <= weaponStats.range / GameConfig.RangeToRadius;
        }

        private void SetSpecialModifiers(List<SpecialModifiers> specialModifiersList)
        {
            for (int i = 0; i < specialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(specialModifiersList[i]);
                modifier.RegisterUser(gameObject);
                switch (modifier.useArea)
                {
                    case ModifierUseArea.OnStart:
                        if (!modifiersOnStart.Contains(modifier))
                            modifiersOnStart.Add(modifier);
                        break;

                    case ModifierUseArea.OnHit:
                        break;

                    case ModifierUseArea.OnGetHit:
                        if (!modifiersOnGetHit.Contains(modifier))
                            modifiersOnGetHit.Add(modifier);
                        break;

                    case ModifierUseArea.OnBuyItem:
                        if (!modifiersOnItemBuy.Contains(modifier))
                            modifiersOnItemBuy.Add(modifier);
                        break;

                    case ModifierUseArea.OnUpdate:
                        break;

                    case ModifierUseArea.OnHealthChange:
                        if (!modifiersOnHealthChange.Contains(modifier))
                            modifiersOnHealthChange.Add(modifier);
                        break;

                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //modifiers.Add();
            }
        }

        private void ActivateModifiers()
        {
            for (int i = 0; i < modifiersOnStart.Count; i++)
            {
                modifiersOnStart[i].ApplyEffect(this);
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

            var damageIncreasePercentage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.Damage);


            weaponStats.specialModifiers = weaponDatsSo.specialModifiersList;

            var data = weaponDatsSo.WeaponData[weaponType];
            weaponStats.damage = data.BaseDamage + extraDamage;
            weaponStats.damage += weaponStats.damage*damageIncreasePercentage;
            weaponStats.range = data.BaseAttackRange;
            weaponStats.attackSpeed = data.BaseAttackSpeed;
            weaponStats.projectileAmount = data.BaseProjectileAmount;
            weaponStats.criticalHitChance = data.BaseCriticalHitChance;
            weaponStats.criticalHitDamage = data.BaseCriticalHitDamage;

            weaponStats.pierceNum = (int)(data.BasePierceNumber +
                                          ScriptDictionaryHolder.Player.stats.GetStat(AllStats.PierceNumber));

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
            EventManager.Instance.SetDistanceBetweenEnemy(distance * GameConfig.RangeToRadius);
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