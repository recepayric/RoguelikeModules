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
    }
}