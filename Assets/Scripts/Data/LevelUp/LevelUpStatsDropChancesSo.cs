using System;
using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.LevelUp
{
    [CreateAssetMenu(fileName = "Data", menuName = "Level Up Data/Level Up Stats Drop Chances", order = 1)]
    public class LevelUpStatsDropChancesSo : SerializedScriptableObject
    {
        public float normalDropRateIncrease;
        public float magicDropRateIncrease;
        public float rareDropRateIncrease;
        public float epicDropRateIncrease;
        public float uniqueDropRateIncrease;

        public List<LevelUpDropData> levelUpDrops;

        public AllStats stat;
        [Header("Values")] public float normalValue;
        public float magicValue;
        public float rareValue;
        public float epicValue;
        public float legendaryValue;

        [Header("Weights")] public float normalWeight;
        public float magicWeight;
        public float rareWeight;
        public float epicWeight;
        public float legendaryWeight;


        [Button]
        public void AddStat()
        {
            Add(stat, ItemRarity.Normal, normalValue, normalWeight);
            Add(stat, ItemRarity.Magic, magicValue, magicWeight);
            Add(stat, ItemRarity.Rare, rareValue, rareWeight);
            Add(stat, ItemRarity.Epic, epicValue, epicWeight);
            Add(stat, ItemRarity.Unique, legendaryValue, legendaryWeight);
        }

        [Button]
        public void CalculatePercentages()
        {
            var totalWeight = 0f;
            var totalPercentage = 0f;
            for (int i = 0; i < levelUpDrops.Count; i++)
            {
                totalWeight += levelUpDrops[i].weight;
            }

            for (int i = 0; i < levelUpDrops.Count; i++)
            {
                levelUpDrops[i].percentage = (levelUpDrops[i].weight / totalWeight) * 100;
                totalPercentage += levelUpDrops[i].percentage;
            }
            
            //Debug.Log("Total Weight: " + totalWeight);
            //Debug.Log("Total Percentage: " + totalPercentage);
        }

        private void Add(AllStats statType, ItemRarity rarity, float value, float baseWeight)
        {
            if (baseWeight <= 0) return;

            var weight = baseWeight;

            switch (rarity)
            {
                case ItemRarity.Normal:
                    weight *= normalDropRateIncrease;
                    break;
                case ItemRarity.Magic:
                    weight *= magicDropRateIncrease;
                    break;
                case ItemRarity.Rare:
                    weight *= rareDropRateIncrease;
                    break;
                case ItemRarity.Epic:
                    weight *= epicDropRateIncrease;
                    break;
                case ItemRarity.Unique:
                    weight *= uniqueDropRateIncrease;
                    break;
            }
            
            var dt = levelUpDrops.Find(t => t.rarity == rarity && t.stat == statType);
            var data = dt ?? new LevelUpDropData();

            data.stat = statType;
            data.rarity = rarity;
            data.value = value;
            data.weight = weight;
            data.baseWeight = baseWeight;

            if (dt == null)
            {
                Debug.Log("New!!!");
                levelUpDrops.Add(data);
            }
        }

        [Button]
        private void UpdateStatPercentages()
        {
            for (int i = 0; i < levelUpDrops.Count; i++)
            {
                Add(levelUpDrops[i].stat, levelUpDrops[i].rarity, levelUpDrops[i].value, levelUpDrops[i].baseWeight);
            }
        }

        [Button]
        private void SetAllRarityWeights(ItemRarity rarity, int newWeight)
        {
            for (int i = 0; i < levelUpDrops.Count; i++)
            {
                if (levelUpDrops[i].rarity != rarity) continue;
                
                Add(levelUpDrops[i].stat, levelUpDrops[i].rarity, levelUpDrops[i].value, newWeight);
            }
        }
    }

    [Serializable]
    public class LevelUpDropData
    {
        public ItemRarity rarity;
        public AllStats stat;
        public float statIncrease;
        public float value;
        public float baseWeight;
        public float weight;
        public float percentage;
    }
}