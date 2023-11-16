using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.StatValue;
using UnityEngine;

namespace Runtime
{
    [Serializable]
    public class Stats
    {
        public Dictionary<AllStats, int> stats;
        [Header("Base Stats")]
        public int baseStrength;
        public int baseDexterity;
        public int baseIntelligence;
        public int baseMagic;
        public int baseMaxHealth;
        public int baseMeleeAttack;
        public int baseRangedAttack;
        public int baseMagicalAttack;
        public int baseRange;
        public int baseDamage;
        public int baseProjectileAmount;
        public int baseCollectRange;
        public int baseMoveSpeed;
        public int baseAttackSpeed;

        [Header("Stats To Display")]
        public int strength;
        public int intelligence;
        public int dexterity;
        public int attackSpeed;
        public int moveSpeed;
        public int maxHealth;
        public int meleeDamage;
        public int rangedDamage;
        public int magicDamage;

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

        private void SetStatValues()
        {   
            SetStatValue(AllStats.Strength, ref strength);
            SetStatValue(AllStats.Intelligence, ref intelligence);
            SetStatValue(AllStats.Dexterity, ref dexterity);
            SetStatValue(AllStats.AttackSpeed, ref attackSpeed);
            SetStatValue(AllStats.MoveSpeed, ref moveSpeed);
            SetStatValue(AllStats.MaxHealth, ref maxHealth);
            SetStatValue(AllStats.MeleeAttack, ref meleeDamage);
            SetStatValue(AllStats.RangedAttack, ref rangedDamage);
            SetStatValue(AllStats.MagicalAttack, ref magicDamage);
        }

        public void CalculateStats()
        {
            var attackSpeedIncrease = stats[AllStats.Dexterity] * StatConverter.DexToAttackSpeed;
            var moveSpeedIncrease = stats[AllStats.Dexterity] * StatConverter.DexToMoveSpeed;
            var rangedDamageIncreaseIncrease = stats[AllStats.Dexterity] * StatConverter.DexToRangedDamage;

            var meleeDamageIncreaseIncrease = stats[AllStats.Strength] * StatConverter.StrToMeleeDamage;
            var healthIncrease = stats[AllStats.Strength] * StatConverter.StrToHealth;

            var magicDamageIncreaseIncrease = stats[AllStats.Intelligence] * StatConverter.IntToMagicDamage;

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
            //Main Stats
            SetStat(AllStats.Strength, baseStrength);
            SetStat(AllStats.Dexterity, baseDexterity);
            SetStat(AllStats.Intelligence, baseIntelligence);
            SetStat(AllStats.Magic, baseMagic);

            SetStat(AllStats.MoveSpeed, baseMoveSpeed);
            SetStat(AllStats.AttackSpeed, baseAttackSpeed);
            SetStat(AllStats.MeleeAttack, baseMeleeAttack);
            SetStat(AllStats.RangedAttack, baseRangedAttack);
            SetStat(AllStats.MagicalAttack, baseMagicalAttack);
            SetStat(AllStats.MaxHealth, baseMaxHealth);
        }

        public void SetStats()
        {
            if (stats == null)
                stats = new Dictionary<AllStats, int>();
            else
                stats.Clear();

            //Main Stats
            AddStat(AllStats.Strength, baseStrength);
            AddStat(AllStats.Dexterity, baseDexterity);
            AddStat(AllStats.Intelligence, baseIntelligence);
            AddStat(AllStats.Magic, baseMagic);

            //Base Stats
            AddStat(AllStats.MagicalAttack, baseMagicalAttack);
            AddStat(AllStats.RangedAttack, baseRangedAttack);
            AddStat(AllStats.Damage, baseDamage);
            AddStat(AllStats.MeleeAttack, baseMeleeAttack);
            AddStat(AllStats.Range, baseRange);
            AddStat(AllStats.MaxHealth, baseMaxHealth);
            AddStat(AllStats.CollectRange, baseCollectRange);

            AddStat(AllStats.MoveSpeed, baseMoveSpeed);
            AddStat(AllStats.AttackSpeed, baseAttackSpeed);

            //Secondary Stats
        }

        private void AddStat(AllStats stat, int value)
        {
            if (value != 0)
            {
                stats.Add(stat, value);
            }
        }

        private void SetStat(AllStats stat, int value)
        {
            if (stats.ContainsKey(stat))
                stats[stat] = value;
            else
                AddStat(stat, value);
        }

        private void IncreaseStat(AllStats stat, int value)
        {
            if (stats.ContainsKey(stat))
                stats[stat] += value;
            else if (value != 0)
                stats.Add(stat, value);
        }

        private void SetStatValue(AllStats stat, ref int value)
        {
            if (stats.ContainsKey(stat))
                value = stats[stat];
            else
                value = 0;
        }
    }
}