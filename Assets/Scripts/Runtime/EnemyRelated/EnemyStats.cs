using System;
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


        private void Start()
        {
        }

        public void SetStats()
        {
            if (ElementalStatus == null) ElementalStatus = new ElementalAilmentStatus();
            if (enemyData == null) return;
            _health = GetComponent<Health>();
            _tower = ScriptDictionaryHolder.CurrentTower;

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
    }
}