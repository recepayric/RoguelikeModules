using System;
using System.Collections.Generic;
using Runtime.Configs;
using Runtime.Enums;
using UnityEngine.Serialization;

namespace Runtime.TowerRelated
{
    [Serializable]
    public class Tower
    {
        public float statIncreaseRatePerFloor = 1.1f;
        public float baseStatIncrease = 1f;

        public ItemRarity towerRarity;
        public int tier;
        public string towerName;
        public List<TowerModifier> TowerModifiers;
        private float _dropRateIncrease;

        public float DropRateIncrease
        {
            get => (int)_dropRateIncrease;
            set => _dropRateIncrease = value;
        }

        private float _experienceRateIncrease = 0;

        public float ExperienceRateIncrease
        {
            get => (int)_experienceRateIncrease;
            set => _experienceRateIncrease = value;
        }

        private float _upgradeCost;
        private float _rerollCost;

        public Tower()
        {
            DropRateIncrease = 0;
            ExperienceRateIncrease = 0;
        }

        public void AddModifier(TowerModifier modifier)
        {
            if (TowerModifiers == null)
                TowerModifiers = new List<TowerModifier>();
            TowerModifiers.Add(modifier);

            DropRateIncrease += modifier.CurrentEffect * MapConfig.ModifyToDropRateMultiplier;
            ExperienceRateIncrease += modifier.CurrentEffect * MapConfig.ModifyToDropRateMultiplier;
        }

        public void CalculateRates()
        {
            DropRateIncrease += 100;
            ExperienceRateIncrease += 100;
        }

        public void ClearTower()
        {
            if (TowerModifiers == null)
                TowerModifiers = new List<TowerModifier>();
            TowerModifiers.Clear();

            DropRateIncrease = 0;
            ExperienceRateIncrease = 0;
        }

        public void SetName(string name)
        {
            towerName = name;
        }

        public void SetTier(int _tier)
        {
            tier = _tier;
        }

        public int GetUpgradeCost()
        {
            int rarityNum = (int)towerRarity;
            int price = 2 + tier * 2;
            price *= (rarityNum + 1);

            _upgradeCost = price;

            if (_upgradeCost <= 0)
                _upgradeCost = 1;

            return (int)_upgradeCost;
        }

        public int getRerollCost()
        {
            int rarityNum = (int)towerRarity;
            int price = 1 + tier;
            price *= (rarityNum + 1) / 2;
            _rerollCost = price;

            if (_rerollCost <= 0)
                _rerollCost = 1;

            return (int)_rerollCost;
        }
    }
}