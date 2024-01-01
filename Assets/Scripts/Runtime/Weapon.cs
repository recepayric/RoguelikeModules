using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Managers;
using Runtime.Modifiers;
using Runtime.ParticleShaderScripts;
using Runtime.WeaponRelated;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    [RequireComponent(typeof(WeaponLevelSystem))]
    [RequireComponent(typeof(WeaponUpgradeTree))]
    public class Weapon : MonoBehaviour, IShooter
    {
        public WeaponLevelSystem weaponLevelSystem;
        public WeaponUpgradeTree weaponUpgradeTree;
        public bool isActivated = false;
        public WeaponDatasSo weaponDatsSo;
        public Weapons weaponType;
        public Stats characterStats;
        public WeaponStats weaponStats;

        private float _timer;
        private float distanceToEnemy;
        public float attackTime;

        public GameObject projectilePrefab;
        public GameObject projectilePoint;

        public CircleCollider2D circleCollider2D;
        public Collider2D colliderHit;

        public List<GameObject> enemiesInRange;
        public bool hasEnemyInRange;
        public GameObject targetEnemy;

        public Dictionary<AllStats, float> statsFromTree;
        public List<SpecialModifiers> specialModifiersFromTree;
        public List<SpecialModifiers> specialModifiersList;
        public List<Modifier> modifiers;
        public List<Modifier> modifiersOnStart;
        public List<Modifier> modifiersOnGetHit;
        public List<Modifier> modifiersOnHealthChange;
        public List<Modifier> modifiersOnItemBuy;

        public static float RotationGlobal;

        public SwirlObject swirlObject;

        public bool isAttacking = false;

        public List<GameObject> styles;
        public int styleCount;


        private void Start()
        {
            circleCollider2D = GetComponent<CircleCollider2D>();
            SetStats();
            //SetSpecialModifiers();
            SetSpecialModifiers(specialModifiersList);
            SetSpecialModifiers(weaponStats.specialModifiers);
            ActivateModifiers();
            UpdateStyle();
        }

        private void Update()
        {
            RotationGlobal += Time.deltaTime * 100f;
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
                _timer += Time.deltaTime;
                if (_timer >= weaponStats.attackSpeed)
                {
                    Attack();
                    _timer = 0;
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
                _timer = 0;
            }

            swirlObject.transform.position = projectilePoint.transform.position;
        }

        public void OnFloorStart()
        {
            SetStats();
            ActivateModifiers();
        }

        public void AddStatFromTree(AllStats stat, float value)
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();

            if (statsFromTree.ContainsKey(stat))
                statsFromTree[stat] += value;
            else
                statsFromTree.Add(stat, value);
            
            SetStats();
        }

        public void AddModifierFromTree(SpecialModifiers specialModifier)
        {
            specialModifiersFromTree.Add(specialModifier);
            AddSpecialModifier(specialModifier);
        }

        public void AddStyle(int number)
        {
            Debug.Log("Don't forget to change your weapon's style!!!");
            styleCount++;
            UpdateStyle();
        }

        private void UpdateStyle()
        {
            for (int i = 0; i < styles.Count; i++)
            {
                if(i == styleCount)
                    styles[i].SetActive(true);
                else
                    styles[i].SetActive(false);
            }
        }
        

        public void UpdateAttackSpeed()
        {
            var attackSpeedBuff = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.AttackSpeed);
            var currentSpeed = weaponDatsSo.WeaponData[weaponType].BaseAttackSpeed;
            var multiplierFromTree = (1 + GetStat(AllStats.AttackSpeed) / 100f);
            if (multiplierFromTree == 0)
                multiplierFromTree = 0.01f;
            currentSpeed = currentSpeed / multiplierFromTree;     

            var multiplier = (1 + attackSpeedBuff / 100f);
            if (multiplier == 0)
                multiplier = 0.01f;
            weaponStats.attackSpeed = currentSpeed / multiplier;
        }

        private bool CanAttack()
        {
            return targetEnemy != null && distanceToEnemy <= weaponStats.range / GameConfig.RangeToRadius;
        }

        private void SetSpecialModifiers(List<SpecialModifiers> pSpecialModifiersList)
        {
            for (int i = 0; i < pSpecialModifiersList.Count; i++)
            {
                AddSpecialModifier(pSpecialModifiersList[i]);
            }
        }

        private void AddSpecialModifier(SpecialModifiers specialModifier)
        {
            var modifier = ModifierCreator.GetModifier(specialModifier);
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
        }

        private void ActivateModifiers()
        {
            for (var i = 0; i < modifiersOnStart.Count; i++)
            {
                modifiersOnStart[i].ApplyEffect(this);
            }
        }

        private void SetStats()
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();
            //extra damages
            var extraDamage = 0f;

            if (weaponType == Weapons.Gun)
                extraDamage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.RangedAttack);

            if (weaponType == Weapons.Sword)
                extraDamage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.MeleeAttack);

            var damageIncreasePercentage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.Damage)/100f;

            weaponStats.specialModifiers = weaponDatsSo.specialModifiersList;
            
            //From Upgrade Tree!!!
            var rangeIncreaseFromTree = GetStat(AllStats.Range);
            var damageIncreaseFromTree = GetStat(AllStats.Damage);
            var attackSpeedIncreaseFromTree = GetStat(AllStats.AttackSpeed);
            var damagePoint = GetStat(AllStats.MagicalAttack);
            damagePoint += GetStat(AllStats.RangedAttack);
            damagePoint += GetStat(AllStats.MeleeAttack);
            

            var projectileFromTree = GetStat(AllStats.ProjectileNumber);
            var bounceFromTree = GetStat(AllStats.BounceNumber);

            var data = weaponDatsSo.WeaponData[weaponType];
            weaponStats.damage = data.BaseDamage + extraDamage + damagePoint;
            weaponStats.damage += weaponStats.damage*damageIncreasePercentage;
            weaponStats.damage += weaponStats.damage*damageIncreaseFromTree;

            weaponStats.range = data.BaseAttackRange + rangeIncreaseFromTree;
            
            weaponStats.attackSpeed = data.BaseAttackSpeed;
            var multiplier = (1 + attackSpeedIncreaseFromTree / 100f);
            if (multiplier == 0)
                multiplier = 0.01f;
            weaponStats.attackSpeed = weaponStats.attackSpeed / multiplier;
            
            weaponStats.projectileAmount = data.BaseProjectileAmount + (int)projectileFromTree;
            weaponStats.bounceNum = (int)bounceFromTree;
            
            weaponStats.criticalHitChance = data.BaseCriticalHitChance;
            weaponStats.criticalHitDamage = data.BaseCriticalHitDamage;

            weaponStats.pierceNum = (int)(data.BasePierceNumber +
                                          ScriptDictionaryHolder.Player.stats.GetStat(AllStats.PierceNumber));

            if (circleCollider2D != null)
                circleCollider2D.radius = (weaponStats.range / GameConfig.RangeToRadius);
        }

        private float GetStat(AllStats stat)
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();
            if (statsFromTree.ContainsKey(stat))
                return statsFromTree[stat];

            return 0;
        }

        public void SetEnemy(GameObject enemy)
        {
            if (enemy == null || !ScriptDictionaryHolder.Enemies[enemy].IsAvailable())
            {
                targetEnemy = null;
                return;
            }

            targetEnemy = enemy;
        }

        public void SetEnemy(GameObject enemy, float distance)
        {
            if (enemy == null || !ScriptDictionaryHolder.Enemies[enemy].IsAvailable())
            {
                targetEnemy = null;
                return;
            }

            targetEnemy = enemy;

            distanceToEnemy = distance;
            EventManager.Instance.SetDistanceBetweenEnemy(distance * GameConfig.RangeToRadius);
        }

        private void Attack()
        {
            if (weaponType != Weapons.Gun)
                return;

            //CheckForEnemy();

            if (targetEnemy == null || !ScriptDictionaryHolder.Enemies[targetEnemy].IsAvailable())
                return;

            //transform.right = targetEnemy.transform.position - transform.position;

            for (int i = 0; i < weaponStats.projectileAmount; i++)
            {
                //todo change this to pool and dictionary.
                var projectile = Instantiate(projectilePrefab);
                projectile.transform.position = projectilePoint.transform.position;
                projectile.transform.right = targetEnemy.transform.position - projectile.transform.position;

                var sc = projectile.GetComponent<Projectile>();

                sc.bounceNum = weaponStats.bounceNum;
                sc.pierceNum = weaponStats.pierceNum;
                sc.criticalHitChance = weaponStats.criticalHitChance / 100f;
                sc.criticalHitDamage = weaponStats.criticalHitDamage;
                sc.weapon = this;
                sc.SetModifiers(modifiers);
                sc.SetMaxDistance(weaponStats.range);
                sc.SetHomingProjectile(weaponStats.hasHomingProjectiles, targetEnemy);
                sc.isRotating = weaponStats.hasRotatingProjectiles;
                sc.SetShooter(this);
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

            targetEnemy = hasEnemyInRange ? enemiesInRange[0] : null;
        }

        private void DealDamage(GameObject enemy)
        {
            var damage = 1;
            var isCriticalHit = Random.Range(0, 1f) <= 0.5f;
            ScriptDictionaryHolder.Enemies[enemy].DealDamage(damage, isCriticalHit);
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

        public float GetDamage()
        {
            return weaponStats.damage;
        }
    }
}