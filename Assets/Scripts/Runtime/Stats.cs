using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.StatValue;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class Stats
    {
        private CharacterDataSo _baseStats;

        public Dictionary<AllStats, float> stats;
        [Header("Base Stats")]
        public float strength;
        public float dexterity;
        public float intelligence;
        public float magic;
        public float maxHealth;
        public float meleeAttack;
        public float rangedAttack;
        public float magicalAttack;
        public float range;
        public float damage;
        public float projectileAmount;
        public float collectRange;
        public float moveSpeed;
        public float attackSpeed;
        public float pierceNum;
        public float pierceDamage;
        public float bounceNum;
        
        public void SetBaseStat(CharacterDataSo baseStats)
        {
            _baseStats = baseStats;
        }

        public void AddStats(Stats statsToAdd)
        {
            var e = statsToAdd.stats.GetEnumerator();
            while (e.MoveNext())
            {
                var stat = e.Current.Key;
                var value = e.Current.Value;

                stats[stat] += value;
            }
        }

        public void SetStatValues()
        {
            SetStatValue(AllStats.Strength, ref strength);
            SetStatValue(AllStats.Dexterity, ref dexterity);
            SetStatValue(AllStats.Intelligence, ref intelligence);
            SetStatValue(AllStats.Magic, ref magic);
            SetStatValue(AllStats.MaxHealth, ref maxHealth);
            SetStatValue(AllStats.MeleeAttack, ref meleeAttack);
            SetStatValue(AllStats.RangedAttack, ref rangedAttack);
            SetStatValue(AllStats.MagicalAttack, ref magicalAttack);
            SetStatValue(AllStats.Range, ref range);
            SetStatValue(AllStats.Damage, ref damage);
            SetStatValue(AllStats.ProjectileNumber, ref projectileAmount);
            SetStatValue(AllStats.CollectRange, ref collectRange);
            SetStatValue(AllStats.MoveSpeed, ref moveSpeed);
            SetStatValue(AllStats.AttackSpeed, ref attackSpeed);
        }

        public void CalculateStats()
        {
            var attackSpeedIncrease = GetStat(AllStats.Dexterity) * StatConverter.DexToAttackSpeed;
            var moveSpeedIncrease = GetStat(AllStats.Dexterity) * StatConverter.DexToMoveSpeed;
            var rangedDamageIncreaseIncrease = GetStat(AllStats.Dexterity) * StatConverter.DexToRangedDamage;

            var meleeDamageIncreaseIncrease = GetStat(AllStats.Strength) * StatConverter.StrToMeleeDamage;
            var healthIncrease = GetStat(AllStats.Strength) * StatConverter.StrToHealth;

            var magicDamageIncreaseIncrease = GetStat(AllStats.Intelligence) * StatConverter.IntToMagicDamage;

            IncreaseStat(AllStats.MoveSpeed, moveSpeedIncrease);
            IncreaseStat(AllStats.AttackSpeed, attackSpeedIncrease);
            IncreaseStat(AllStats.RangedAttack, rangedDamageIncreaseIncrease);
            IncreaseStat(AllStats.MeleeAttack, meleeDamageIncreaseIncrease);
            IncreaseStat(AllStats.MaxHealth, healthIncrease);
            IncreaseStat(AllStats.MagicalAttack, magicDamageIncreaseIncrease);
            
            SetStatValues();
        }

        public void SetBaseStats()
        {
            SetStats();
            foreach (var stat in _baseStats.BaseStats)
            {
                AddStat(stat.Key, stat.Value);
            }
        }

        public void SetStats()
        {
            if (stats == null)
                stats = new Dictionary<AllStats, float>();
            else
                stats.Clear();

            //Main Stats
            AddStat(AllStats.Strength, 0);
            AddStat(AllStats.Dexterity, 0);
            AddStat(AllStats.Intelligence, 0);
            AddStat(AllStats.Magic, 0);

            //Base Stats
            AddStat(AllStats.MagicalAttack, 1);
            AddStat(AllStats.RangedAttack, 1);
            AddStat(AllStats.Damage, 0);
            AddStat(AllStats.MeleeAttack, 1);
            AddStat(AllStats.Range, 0);
            AddStat(AllStats.MaxHealth, 10);
            AddStat(AllStats.CollectRange, 3);

            AddStat(AllStats.MoveSpeed, 0);
            AddStat(AllStats.AttackSpeed, 0);
            AddStat(AllStats.PierceNumber, 0);
            AddStat(AllStats.PierceDamage, 0.5f);
            AddStat(AllStats.BounceNumber, 0);

            //Secondary Stats
        }

        private void AddStat(AllStats stat, float value)
        {
            if (value != 0)
            {
                stats.Add(stat, value);
            }
        }

        private void SetStat(AllStats stat, float value)
        {
            if (stats.ContainsKey(stat))
                stats[stat] = value;
            else
                AddStat(stat, value);
        }

        public void IncreaseStat(AllStats stat, float value)
        {
            if (stats.ContainsKey(stat))
                stats[stat] += value;
            else if (value != 0)
                stats.Add(stat, value);
        }

        private void SetStatValue(AllStats stat, ref float value)
        {
            if (stats.ContainsKey(stat))
                value = stats[stat];
            else
                value = 0;
        }

        public float GetStat(AllStats stat)
        {
            var val = 0f;
            stats.TryGetValue(stat, out val);
            return val;
        }
    }
}