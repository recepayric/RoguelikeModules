using System.Collections.Generic;
using System.Linq;
using Runtime;
using Runtime.Enums;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Item = Runtime.ItemsRelated.Item;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemDataObject", order = 2)]
    public class ItemDataSo : SerializedScriptableObject
    {
        public Item item;
        
        public List<Item> itemData;
        public List<Item> itemData2;

        [Button]
        public void ResetItemQuantities()
        {
            for (int i = 0; i < itemData.Count; i++)
            {
                itemData[i].quantity = 0;
            }
        }
        [Button]
        public void SetStatsForItems()
        {
            for (int i = 0; i < itemData.Count; i++)
            {
                //itemData[i].itemStats.SetStats();
            }
        }

        public ItemRarity Rarity;
        public Sprite texture;
        public string name;
        public string description;
        //public Stats Stats;
        public Dictionary<AllStats, float> statsToAdd;

        [Button]
        public void AddItem()
        {
            Item item = new Item();
            item.name = name;
            item.stats = new Dictionary<AllStats, float>();
            item.statNames = new List<AllStats>();
            item.statValues = new List<float>();
            item.statNames.AddRange(statsToAdd.Keys.ToList());
            item.statValues.AddRange(statsToAdd.Values.ToList());
            //item.stats.AddRange(statsToAdd);
            item.itemIcon = texture;
            item.rarity = Rarity;
            item.description = description;
            
            texture = null;
            statsToAdd.Clear();
            name = "";
            itemData.Add(item);
            //item.itemStats.AddStats(Stats);
        }

        

        public int addAmount;
        public List<AllStats> statsToAddRandom;
        public string[] suffixes;
        public string[] prefixes;
        public string[] names;
        public Sprite[] texturesToAdd;

        [Button]
        public void AddRandomItems()
        {
            itemData2.Clear();
            for (int i = 0; i < addAmount; i++)
            {
                AddItemRandom();
            }
        }

        private void AddItemRandom()
        {
            var sprite = texturesToAdd[Random.Range(0, texturesToAdd.Length)];
            var name = names[Random.Range(0, names.Length)];
            var odd = Random.Range(0, 1f);
            if (odd < 0.5f)
            {
                var pref = prefixes[Random.Range(0, prefixes.Length)];
                name = pref + " " + name;
            }else 
            {
                var suff = suffixes[Random.Range(0, prefixes.Length)];
                name = name + " " + suff;
            }

            var statAddAmount = Random.Range(1, 4);
            var copyStats = statsToAddRandom.ToList();
            
            Item item = new Item();
            item.name = name;
            item.stats = new Dictionary<AllStats, float>();
            item.statNames = new List<AllStats>();
            item.statValues = new List<float>();
            //item.stats.AddRange(statsToAdd);
            item.itemIcon = sprite;
            item.rarity = Rarity;
            
            for (int i = 0; i < statAddAmount; i++)
            {
                var rand = Random.Range(0, copyStats.Count);
                var randAmount = Random.Range(-15, 15);
                item.statNames.Add(copyStats[rand]);
                item.statValues.Add(randAmount);
            }
            itemData2.Add(item);
        }
    }
}