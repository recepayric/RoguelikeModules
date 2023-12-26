using System;
using System.Collections.Generic;
using Runtime.Configs;
using UnityEngine.Serialization;

namespace Runtime.TowerRelated
{
    [Serializable]
    public class Tower
    {
        public float statIncreaseRatePerFloor = 1.1f;
        public float baseStatIncrease = 1f;
        
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
        public float ExperienceRateIncrease  {
            get => (int)_experienceRateIncrease;
            set => _experienceRateIncrease = value;
        }

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
    }
}