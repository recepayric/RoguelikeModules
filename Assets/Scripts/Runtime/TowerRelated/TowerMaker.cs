using System;
using System.Collections.Generic;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.TowerRelated
{
    public class TowerMaker : MonoBehaviour
    {
        public List<TowerModifier> TowerModifiers;
        public List<TowerModifier> TowerModifiers2;
        public int tier;
        public ItemRarity rarity;

        public List<Tower> towers;

        public List<string> modifiers;

        public float towerMinStatMulti;
        public float towerMaxStatMulti;

        private void Awake()
        {
            towers = new List<Tower>();
            InitialiseTowerModifiers();
            InitialiseBaseTowers();
            EventManager.Instance.CreateTowerEvent += OnCreateTower;
            EventManager.Instance.PrepareTowerEvent += OnPrepareTower;
        }
        
        
        private void OnDestroy()
        {
            EventManager.Instance.CreateTowerEvent -= OnCreateTower;
            EventManager.Instance.PrepareTowerEvent -= OnPrepareTower;
        }

        private void OnPrepareTower(int tierToPrepare)
        {
            DictionaryHolder.CurrentTower = towers[tierToPrepare - 1];
        }

        private void OnCreateTower(int tierToCreate)
        {
            tier = tierToCreate;
            CreateTower(towers[tier-1]);
        }

        [Button]
        public void CreateTower(Tower tower)
        {
            if (TowerModifiers == null)
                TowerModifiers = new List<TowerModifier>();
            
            if (TowerModifiers2 == null)
                TowerModifiers2 = new List<TowerModifier>();
            
            TowerModifiers2.Clear();
            TowerModifiers2.AddRange(TowerModifiers);
            modifiers.Clear();
            
            tower.ClearTower();

            for (int i = 0; i < MapConfig.ModifiersPerTier[tower.tier-1]; i++)
            {
                var total = TowerModifiers2.Count;
                var index = Random.Range(0, total);
                var modifier = TowerModifiers2[index];
                TowerModifiers2.Remove(modifier);
                modifier.RandomizeValue(tier);
                modifiers.Add(modifier.TextToShow);
                tower.AddModifier(modifier);
                tower.statIncreaseRatePerFloor = Random.Range(1.1f, 1.5f);
                tower.baseStatIncrease = Random.Range(towerMinStatMulti, towerMaxStatMulti);
            }
            tower.CalculateRates();
        }
        
        private void InitialiseBaseTowers()
        {
            for (int i = 0; i < 16; i++)
            {
                Tower tower = new Tower();
                tower.SetTier(i+1);
                tower.SetName("Base Tower");
                CreateTower(tower);
                towers.Add(tower);
            }
        }
        
        [Button]
        public void InitialiseTowerModifiers()
        {
            if (TowerModifiers == null)
                TowerModifiers = new List<TowerModifier>();
            
            if (TowerModifiers2 == null)
                TowerModifiers2 = new List<TowerModifier>();
            
            TowerModifiers.Clear();
            
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterHealthIncrease, 10, 30).SetText(TempModifierTexts.MonsterHealthIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterSpeedIncrease, 10, 30).SetText(TempModifierTexts.MonsterSpeedIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterDamageIncrease, 10, 30).SetText(TempModifierTexts.MonsterDamageIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterRangeIncrease, 10, 30).SetText(TempModifierTexts.MonsterRangeIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterDefenceIncrease, 10, 30).SetText(TempModifierTexts.MonsterDefenceIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterEvasionIncrease, 10, 30).SetText(TempModifierTexts.MonsterEvasionIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterAttackSpeedIncrease, 10, 30).SetText(TempModifierTexts.MonsterAttackSpeedIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterAlwaysDealCriticalHit, 10, 30).SetText(TempModifierTexts.MonsterAlwaysDealCriticalHit));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterDontTakeCriticalHit, 10, 30).SetText(TempModifierTexts.MonsterDontTakeCriticalHit));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCriticalDamageIncrease, 10, 30).SetText(TempModifierTexts.MonsterCriticalDamageIncrease));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterHitsShock, 10, 30).SetText(TempModifierTexts.MonsterHitsShock));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterHitsFreeze, 10, 30).SetText(TempModifierTexts.MonsterHitsFreeze));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterHitsBurn, 10, 30).SetText(TempModifierTexts.MonsterHitsBurn));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCantBeFrozen, 10, 30).SetText(TempModifierTexts.MonsterCantBeFrozen));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCantBeBurn, 10, 30).SetText(TempModifierTexts.MonsterCantBeBurn));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCantBeShocked, 10, 30).SetText(TempModifierTexts.MonsterCantBeShocked));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterHealthRegen, 10, 30).SetText(TempModifierTexts.MonsterHealthRegen));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCantTakeMagicalDamage, 10, 30).SetText(TempModifierTexts.MonsterCantTakeMagicalDamage));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCantTakeMeleeDamage, 10, 30).SetText(TempModifierTexts.MonsterCantTakeMeleeDamage));
            TowerModifiers.Add(new TowerModifier(TowerMonsterModifiers.MonsterCantTakeRangedDamage, 10, 30).SetText(TempModifierTexts.MonsterCantTakeRangedDamage));
        }
        
    }

    public enum TowerMonsterModifiers{
        MonsterHealthIncrease,
        MonsterSpeedIncrease,
        MonsterDamageIncrease,
        MonsterRangeIncrease,
        MonsterDefenceIncrease,
        MonsterEvasionIncrease,
        MonsterAttackSpeedIncrease,
        MonsterAlwaysDealCriticalHit,
        MonsterDontTakeCriticalHit,
        MonsterCriticalDamageIncrease,
        MonsterHitsShock,
        MonsterHitsFreeze,
        MonsterHitsBurn,
        MonsterCantBeFrozen,
        MonsterCantBeBurn,
        MonsterCantBeShocked,
        MonsterHealthRegen,
        MonsterCantTakeMagicalDamage,
        MonsterCantTakeMeleeDamage,
        MonsterCantTakeRangedDamage,
    }

    public enum TowerPlayerModifiers
    {
        PlayerTakesMoreDamage,
        PlayerHasReducedSpeed,
        PlayerHasReducedAttackSpeed,
        PlayerHasReducedArmor,
        PlayerHasReducedEvasion,
        PlayerHasReducedHealthRegen,
        PlayerHasReducedMagicDamage,
        PlayerHasReducedRangedDamage,
        PlayerHasReducedMeleeDamage,
        PlayerHasReducedProjectileAmount,
        PlayerTakesMoreBurnDamage,
        PlayerTakesMoreFreezeEffect,
        PlayerTakesMoreShockEffect,
        PlayerTakesLongerBurnDamage,
        PlayerTakesLongerFreezeEffect,
        PlayerTakesLongerShockEffect,
        PlayerHasReducedCriticalHitChance,
        PlayerHasReducedCriticalDamage,
        PlayerIsUnlucky,
        PlayerSpellsHasMoreCooldown,
    }

    public enum TowerEffectModifiers
    {
        RandomSpikeFromGround,
        HasBurningGround,
        HasFreezingGround,
        HasShockGround,
    }
}