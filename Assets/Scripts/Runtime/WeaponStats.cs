using System;
using System.Collections.Generic;
using Data;
using Data.WeaponDataRelated;
using Runtime.Enums;
using Runtime.Modifiers;
using Runtime.SpellsRelated;
using Runtime.SpellsRelated.Cast;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime
{
    [Serializable]
    public class WeaponStats
    {
        public WeaponTypes weaponType;
        
        public float explosionDamageMultiplier;
        public bool explodingProjectile;
        public bool criticalHitForSure;
        public float doubleDamageChance;

        public bool hasSphereProjectile;
        public float damageReductionOnPierce;
        
        
        public float attackSpeed;
        public float damage;
        public float range;
        public int projectileAmount;
        public int criticalHitChance;
        public int criticalHitDamage;

        public int bounceNum;
        public int pierceNum;
        public int rotationDistanceFromPlayer;

        public bool hasHomingProjectiles;
        public bool hasRotatingProjectiles;
        public bool isRotatinSword;

        public List<Spells> spellNames;
        public List<SpellV2> spells;

        public PoolKeys rotatingWeaponKey;

        public List<SpecialModifiers> specialModifiers;
        public Dictionary<AllStats, float> statsFromTree = new Dictionary<AllStats, float>();
        public WeaponDataSo _weaponDataSo;
        public Sounds attackSound;

        public float totalDamageWithThisWeapon;
        public float totalDamageInThisFloor;
        public float totalDamageInThisTower;
        public int totalKillInThisTower;
        public int totalKillInThisFloor;
        public int totalKillWithoutGotHit;
        public int totalKillWithThisWeapon;
        public int totalAttackNumber;
        public int totalAttackNumberWithoutGettingHit;

        //Elemental Values
        [ShowInInspector] public bool addBurn;
        [ShowInInspector] public bool addFreeze;
        [ShowInInspector] public bool addShock;
        [ShowInInspector] public bool addBleed;
        [ShowInInspector] public bool addStun;

        [ShowInInspector] public float bleedChance;
        [ShowInInspector] public float stunChance;

        [ShowInInspector] public float burnTime;
        [ShowInInspector] public float burnDamage;

        [ShowInInspector] public float freezeTime;
        [ShowInInspector] public float freezeEffect;

        [ShowInInspector] public float shockTime;
        [ShowInInspector] public float shockEffect;
        
        [ShowInInspector] public float bleedTime;
        [ShowInInspector] public float bleedDamage;

        [ShowInInspector] public float stunTime;
        [ShowInInspector] public float stunEffect;
        
        [ShowInInspector] public int burnSpreadAmount;
        [ShowInInspector] public int burnSpreadMultiplier = 1;

        public void CreateSpells()
        {
            for (int i = 0; i < spellNames.Count; i++)
            {
                switch (spellNames[i])
                {
                    case Spells.ExplosionRune:
                        var spell = new CarveRune();
                        spells.Add(spell);
                        break;
                }
            }
        }

        public void UpdateAttackSpeed()
        {
            var attackSpeedBuff = DictionaryHolder.Player.stats.GetStat(AllStats.AttackSpeed);
            var currentSpeed = _weaponDataSo.BaseAttackSpeed;
            var multiplierFromTree = (1 + GetStat(AllStats.AttackSpeed) / 100f);
            if (multiplierFromTree == 0)
                multiplierFromTree = 0.01f;
            currentSpeed = currentSpeed / multiplierFromTree;

            var multiplier = (1 + attackSpeedBuff / 100f);
            if (multiplier == 0)
                multiplier = 0.01f;
            attackSpeed = currentSpeed / multiplier;
        }

        public void SetStats()
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();

            var data = _weaponDataSo;
            rotatingWeaponKey = data.RotatingWeaponKey;
            rotationDistanceFromPlayer = data.rotationDistanceFromPlayer;
            //extra damages
            var extraDamage = 0f;

            if (data.WeaponType == WeaponType.Wand)
                extraDamage = DictionaryHolder.Player.stats.GetStat(AllStats.MagicalAttack);
            else if (data.WeaponType == WeaponType.Bow)
                extraDamage = DictionaryHolder.Player.stats.GetStat(AllStats.RangedAttack);
            else if (data.WeaponType == WeaponType.Sword)
                extraDamage = DictionaryHolder.Player.stats.GetStat(AllStats.MeleeAttack);

            var damageIncreasePercentage = DictionaryHolder.Player.stats.GetStat(AllStats.Damage) / 100f;

            specialModifiers = data.specialModifiersList;

            spellNames = data.spells;

            attackSound = data.attackSound;

            //From Upgrade Tree!!!
            var rangeIncreaseFromTree = GetStat(AllStats.Range);
            var damageIncreaseFromTree = GetStat(AllStats.Damage);
            var attackSpeedIncreaseFromTree = GetStat(AllStats.AttackSpeed);
            var damagePoint = GetStat(AllStats.MagicalAttack);
            damagePoint += GetStat(AllStats.RangedAttack);
            damagePoint += GetStat(AllStats.MeleeAttack);


            var projectileFromTree = GetStat(AllStats.ProjectileNumber);
            var bounceFromTree = GetStat(AllStats.BounceNumber);
            Debug.Log("bounce from tree:  " + bounceFromTree);
            Debug.Log("extra damage:  " + damagePoint);

            //var data = weaponDatsSo.WeaponData[weaponType];
            damage = data.BaseDamage + extraDamage + damagePoint;
            damage += damage * damageIncreasePercentage;
            damage += damage * damageIncreaseFromTree;

            range = data.BaseAttackRange + rangeIncreaseFromTree;

            attackSpeed = data.BaseAttackSpeed;
            var multiplier = (1 + attackSpeedIncreaseFromTree / 100f);
            if (multiplier == 0)
                multiplier = 0.01f;
            attackSpeed = attackSpeed / multiplier;

            projectileAmount = data.BaseProjectileAmount + (int)projectileFromTree;
            bounceNum = data.BaseBounceNumber + (int)bounceFromTree;

            criticalHitChance = data.BaseCriticalHitChance;
            criticalHitDamage = data.BaseCriticalHitDamage;

            pierceNum = (int)(data.BasePierceNumber +
                              DictionaryHolder.Player.stats.GetStat(AllStats.PierceNumber));
            
            //Stat From Tree Part Mainly!!!
            burnSpreadAmount = (int)(DictionaryHolder.Player.stats.GetStat(AllStats.BurnSpreadAmount) + GetStat(AllStats.BurnSpreadAmount));
            burnSpreadAmount *= burnSpreadMultiplier;
        }

        public float GetStat(AllStats stat)
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();
            if (statsFromTree.ContainsKey(stat))
                return statsFromTree[stat];

            return 0;
        }
        
        public void AddStatFromTree(AllStats stat, float value)
        {
            if (statsFromTree == null)
                statsFromTree = new Dictionary<AllStats, float>();

            if (statsFromTree.ContainsKey(stat))
                statsFromTree[stat] += value;
            else
                statsFromTree.Add(stat, value);
            
            
            foreach (var stats in statsFromTree)
            {
                Debug.Log("Added the stat: " + stats.Key + "   " +stats.Value);
            }
            
            SetStats();
        }
    }
}