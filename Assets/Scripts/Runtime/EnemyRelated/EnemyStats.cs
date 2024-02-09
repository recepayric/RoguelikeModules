using System;
using System.Collections.Generic;
using Data.EnemyDataRelated;
using Runtime.Enums;
using Runtime.PlayerRelated;
using Runtime.StatValue;
using Runtime.TowerRelated;
using UnityEngine;

namespace Runtime.EnemyRelated
{
    /// <summary>
    /// Attach to enemy object only
    /// Responsible for calculating, and sharing enemy stats for the object.
    /// Other behaviours can retrieve data from this.
    /// Updates when enemy is spawned!
    /// </summary>
    public class EnemyStats : MonoBehaviour
    {
        [SerializeField] private EnemyData enemyData;
        private Health _health;
        private Tower _tower;

        [Header("Stats")] public float currentHealth;
        public float currentMaxHealth;
        public float currentHealthRegen;
        public float currentSpeed;
        public float currentDamage;
        public float currentAttackSpeed;
        public float currentEvasion;
        public float currentDefence;
        public float currentAttackRange;
        public float currentMaxAttackRange;
        public float currentProjectileNumber;
        public float currentCriticalHitChance;
        public float currentCriticalHitMultiplier;
        public AttackType AttackType;

        [Header("Special Stats")] public bool isImmuneToCriticalHits;
        public bool alwaysDealCriticalHit;
        public float criticalDamageIncrease;

        public bool isImmuneToMeleeDamage;
        public bool isImmuneToRangedDamage;
        public bool isImmuneToMagicDamage;

        public ElementalAilmentStatus ElementalStatus;
        public bool hitsBurn;
        public bool hitsFreeze;
        public bool hitsShock;

        public float burnDamage;
        public float freezeEffect;
        public float shockEffect;

        public Dictionary<AllStats, float> StatsToBuff;
        public PoolKeys explosionKey;

        //todo move these effects here from Enemy class!!!!!!!
        //Self Stats
        //Ailments
        [Header("Ailment Objects")] public GameObject burnAilmentObject;
        public GameObject freezeAilmentObject;
        public GameObject shockAilmentObject;

        [Header("Ailments")] public bool isBurning;
        public bool isFrozen;
        public bool isShocked;

        //Ailment Times
        [Header("Ailment Times")] public float burnTime;
        public float freezeTime;
        public float shockTime;

        //Ailment Effects
        [Header("Ailment Effects")] public float burningDamagePerSecond;
        public float freezeEffectTaken;
        public float shockEffectTaken;
        
        
        private void Start()
        {
        }

        public void SetStats()
        {
            if (ElementalStatus == null) ElementalStatus = new ElementalAilmentStatus();
            if (enemyData == null) return;
            _health = GetComponent<Health>();
            _tower = DictionaryHolder.CurrentTower;

            currentDamage = enemyData.baseDamage;
            currentMaxHealth = enemyData.baseHealth;
            currentHealth = currentMaxHealth;
            currentSpeed = enemyData.baseMoveSpeed;
            currentAttackSpeed = enemyData.baseAttackSpeed;
            currentAttackRange = enemyData.baseAttackRange;
            currentMaxAttackRange = enemyData.baseMaxAttackRange;
            currentEvasion = enemyData.baseEvasion;
            currentDefence = enemyData.baseDefence;
            AttackType = enemyData.attackType;
            currentProjectileNumber = enemyData.baseProjectileNumber;
            explosionKey = enemyData.explosionKey;

            SetFloorStats();
            SetTowerStats(_tower);

            currentHealth = currentMaxHealth;
            _health.SetMaxHealth(currentMaxHealth);
            _health.UpdateHealthBar();
        }

        private void SetFloorStats()
        {
            if (_tower == null) return;
            var floor = GameController.instance.enemySpawner.floorNumber;
            var increase = _tower.baseStatIncrease * Mathf.Pow(_tower.statIncreaseRatePerFloor, floor);
            currentDamage *= increase;
            currentMaxHealth *= increase;
            currentSpeed *= increase;
            //currentAttackSpeed *= increase;
            //currentAttackRange *= increase;
            //currentMaxAttackRange *= increase;
            currentEvasion *= increase;
            currentDefence *= increase;
        }

        private void SetTowerStats(Tower tower)
        {
            if (_tower == null) return;
            for (int i = 0; i < tower.TowerModifiers.Count; i++)
            {
                TowerStatApplier.ApplyStatToEnemy(this, tower.TowerModifiers[i]);
            }
        }

        private void SetAilmentEffects()
        {
            ElementalStatus.burnDamage = currentDamage / 3f;
            ElementalStatus.freezeEffect = 30;
            ElementalStatus.shockEffect = 30;
        }

        public void AddStatBuff(AllStats stat, float amount)
        {
            if (StatsToBuff == null)
                StatsToBuff = new Dictionary<AllStats, float>();
            
            if(StatsToBuff.ContainsKey(stat))
                RemoveBuff(stat);
            
            //todo health will have different approach!!!!

            var increase = 0f;
            var percentage = amount / 100f;
            
            if (stat == AllStats.Damage)
            {
                increase = currentDamage * percentage;
                currentDamage += increase;
            }else if (stat == AllStats.AttackSpeed)
            {
                increase = -currentAttackSpeed * percentage;
                currentAttackSpeed += increase;
            }else if (stat == AllStats.MaxHealth)
            {
                increase = currentMaxHealth * percentage;
                currentMaxHealth += increase;
                currentHealth += increase;
                _health.SetMaxHealth(currentMaxHealth);
                _health.UpdateHealth(currentHealth);
            }else if (stat == AllStats.Range)
            {
                increase = currentAttackRange * percentage;
                currentAttackRange += increase;
            }
            
            StatsToBuff.Add(stat, increase);
        }

        public void RemoveBuff(AllStats stat)
        {
            if (StatsToBuff == null) return;
            if (!StatsToBuff.ContainsKey(stat)) return;
            
            if (stat == AllStats.Damage)
            {
                currentDamage -= StatsToBuff[stat];
            }else if (stat == AllStats.AttackSpeed)
            {
                currentAttackSpeed -= StatsToBuff[stat];
            }else if (stat == AllStats.MaxHealth)
            {
                currentMaxHealth -= StatsToBuff[stat];
                currentHealth -= StatsToBuff[stat];
                if (currentHealth < 0)
                    currentHealth = 1;
                _health.SetMaxHealth(currentMaxHealth);
                _health.UpdateHealth(currentHealth);
            }else if (stat == AllStats.Range)
            {
                currentAttackRange -= StatsToBuff[stat];
            }

            StatsToBuff.Remove(stat);
        }
    }
}