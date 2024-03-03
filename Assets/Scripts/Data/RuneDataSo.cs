using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Misc;
using Runtime.UIRelated.RuneRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/RuneData", order = 1)]
    public class RuneDataSo : SerializedScriptableObject
    {
        public Dictionary<BodyParts, List<RuneSlotData>> runeSlots;
        
        public StatTierDetails _statTierDetails;
        public Dictionary<ItemRarity, int> statNumberForRarity = new Dictionary<ItemRarity, int>();
        public float itemLevelStatIncrease;
        public Dictionary<BodyPartsGenral, List<AllStats>> bodyPartsAndAvailableStats;
        public List<Rune> runes;

        public int runeNumberToCreate;

        [Button]
        public void CreateRune()
        {
            Rune rune = new Rune();
            rune.rune_id = GetId();
            rune.stats = new List<StatDetailed>();
            
            var itemLevel = Random.Range(1, 16);
            var itemRarity = (ItemRarity)Random.Range(0, 5);
            var bodyPart = (BodyPartsGenral)Random.Range(0, 4);
            var statsAvailable = new List<AllStats>();
            statsAvailable.AddRange(bodyPartsAndAvailableStats[bodyPart]);

            int totalStatNumber = statNumberForRarity[itemRarity];

            var maxTier = Mathf.Min(3, itemLevel / 2);

            for (int i = 0; i < totalStatNumber; i++)
            {
                if (statsAvailable.Count == 0) break;
                int statIndex = Random.Range(0, statsAvailable.Count);
                var stat = statsAvailable[statIndex];
                var statTier = Random.Range(0, maxTier);
                
                StatDetailed detailedStat = new StatDetailed();
                detailedStat.stat = stat;
                detailedStat.tierNumber = statTier;
                SetStatValue(detailedStat);

                rune.stats.Add(detailedStat);
                
                statsAvailable.RemoveAt(statIndex);
            }

            rune.BodyPart = bodyPart;
            rune.runeRarity = itemRarity;
            rune.rune_level = itemLevel;

            runes.Add(rune);
        }

        private void SetStatValue(StatDetailed stat)
        {
            Debug.Log(stat.stat);
            var val = _statTierDetails.statValues2[stat.stat][stat.tierNumber];
            stat.maxValue = val.maxStat;
            stat.minValue = val.minStat;
            var value = Random.Range(stat.minValue, stat.maxValue);
            stat.currentValue = value;
        }

        private int GetId()
        {
            var id = 0;
            var canAdd = true;
            do
            {
                id = Random.Range(100, 10000000);
                for (int i = 0; i < runes.Count; i++)
                {
                    if (runes[i].rune_id == id)
                    {
                        canAdd = false;
                        break;
                    }
                }
            } while (!canAdd);

            return id;
        }
    }

    [Serializable]
    public class Rune
    {
        public int rune_id;
        public int rune_level;
        public int runeEquippedSlot;
        public bool isEquipped;
        public ItemRarity runeRarity;
        public BodyPartsGenral BodyPart;
        public BodyParts equippedBodyPart;
        public List<StatDetailed> stats;
        public Sprite itemIcon;
    }

    [Serializable]
    public class RuneSlotData
    {
        public int levelRequirement;
        public Rune equippedRune;
    }
}