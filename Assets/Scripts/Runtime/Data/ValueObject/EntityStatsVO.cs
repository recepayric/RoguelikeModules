using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;


namespace Runtime
{
    [Serializable]
    public class EntityStatsVO
    {
        public string name;
        public event UnityAction StatsChangedEvent;
        private bool initialised = false;
        
        public Dictionary<AllStats, ObscuredFloat> Stats { get; private set; }
        public Dictionary<AllStats, ObscuredFloat> BaseStats { get; private set; }
        public Dictionary<AllStats, ObscuredFloat> ItemStats { get; private set; }
        public Dictionary<AllStats, ObscuredFloat> SecondaryStats { get; private set; }
        public Dictionary<AllStats, ObscuredFloat> LevelUpStats { get; private set; }

        [Header("Base Stats")]
        public ObscuredFloat strength;
        public ObscuredFloat dexterity;
        public ObscuredFloat intelligence;
        public ObscuredFloat magic;
        public ObscuredFloat maxHealth;
        public ObscuredFloat armor;
        public ObscuredFloat evasion;
        public ObscuredFloat meleeAttack;
        public ObscuredFloat rangedAttack;
        public ObscuredFloat magicalAttack;
        public ObscuredFloat range;
        public ObscuredFloat damage;
        public ObscuredFloat projectileAmount;
        public ObscuredFloat collectRange;
        public ObscuredFloat moveSpeed;
        public ObscuredFloat attackSpeed;
        public ObscuredFloat pierceNum;
        public ObscuredFloat pierceDamage;
        public ObscuredFloat bounceNum;

        public EntityStatsVO()
        {
            
        }

        public void initialize()
        {
            Stats = new Dictionary<AllStats, ObscuredFloat>();
            BaseStats = new Dictionary<AllStats, ObscuredFloat>();
            SecondaryStats = new Dictionary<AllStats, ObscuredFloat>();
            ItemStats = new Dictionary<AllStats, ObscuredFloat>();
            LevelUpStats = new Dictionary<AllStats, ObscuredFloat>();
            InitializeStats();
            SetBaseStats();
        }

        public void SetSecondaryStat(AllStats stat, ObscuredFloat value)
        {
            SecondaryStats[stat] = value;
        }

        public void CalculateFinalStats()
        {
            foreach (var stat in BaseStats)
            {
                var statAmount = stat.Value 
                                 + SecondaryStats[stat.Key] 
                                 + ItemStats[stat.Key] 
                                 + LevelUpStats[stat.Key];
                
                Stats[stat.Key] = statAmount;
            }

            SetShowStats();
        }

        private void SetShowStats()
        {
            strength = Stats[AllStats.Strength];
            dexterity = Stats[AllStats.Dexterity];
            intelligence = Stats[AllStats.Intelligence];
            attackSpeed = Stats[AllStats.AttackSpeed];
            moveSpeed = Stats[AllStats.MoveSpeedIncrease];
            maxHealth = Stats[AllStats.MaxHealth];
            armor = Stats[AllStats.Armor];
            evasion = Stats[AllStats.Evasion];
            pierceDamage = Stats[AllStats.PierceNumber];
            bounceNum = Stats[AllStats.BounceNumber];
            projectileAmount = Stats[AllStats.ProjectileNumber];
            
        }

        public void StatsChanged()
        {
            StatsChangedEvent?.Invoke();
        }

        private void InitializeStats()
        {
            foreach (AllStats stat in Enum.GetValues(typeof(AllStats)))
            {
                BaseStats[stat] = 0;
                Stats[stat] = 0;
                SecondaryStats[stat] = 0;
                ItemStats[stat] = 0;
                LevelUpStats[stat] = 0;
            }
            initialised = true;
        }

        public void SetBaseStats()
        {
            if (initialised)
            {
                ResetStats();
            }
        }

        public void AddStats(Dictionary<AllStats, ObscuredFloat> statsToAdd)
        {
            foreach (var stat in statsToAdd)
            {
                IncreaseBaseStat(stat.Key, stat.Value);
            }
            
            StatsChanged();
        }

        public void ResetItemStats()
        {
            foreach (AllStats stat in Enum.GetValues(typeof(AllStats)))
            {
                ItemStats[stat] = 0;
            }
        }

        public void AddStatFromItem(AllStats stat, ObscuredFloat value)
        {
            ItemStats[stat] += value;
        }
        
        public void AddStatFromLevelUp(AllStats stat, ObscuredFloat value)
        {
            LevelUpStats[stat] += value;
        }

        public void ResetStats()
        {
        }

        public void CalculateStats()
        {
            UpdateStatValues();
        }

        public void UpdateStatValues()
        {
            
        }

        public void SetInitialStats()
        {
            
        }

        private void SetStatValue(AllStats stat, ref ObscuredFloat value)
        {
            if (BaseStats.ContainsKey(stat))
            {
                value = BaseStats[stat];
            }
            else
            {
                value = 0;
            }
        }

        public ObscuredFloat GetStat(AllStats stat)
        {
            return Stats.TryGetValue(stat, out var value) ? value : 0;
        }
        
        public ObscuredFloat GetBaseStat(AllStats stat)
        {
            var baseValue = BaseStats[stat];
            var itemValue = ItemStats[stat];
            var levelUpValue = LevelUpStats[stat];
            return baseValue + itemValue + levelUpValue;
        }

        public void SetStat(AllStats stat, ObscuredFloat value)
        {
            BaseStats[stat] = value;
        }

        public void IncreaseBaseStat(AllStats stat, ObscuredFloat value)
        {
            if (BaseStats.ContainsKey(stat))
            {
                BaseStats[stat] += value;
            }
            else
            {
                BaseStats[stat] = value;
            }
        }
        
        public void IncreaseStat(AllStats stat, ObscuredFloat value)
        { 
            Stats[stat] += value;
        }
    }
}
