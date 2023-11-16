using System.Collections.Generic;
using Runtime;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Item = Runtime.ItemsRelated.Item;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ItemDataObject", order = 2)]
    public class ItemDataSo : SerializedScriptableObject
    {
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
                itemData[i].itemStats.SetStats();
            }
        }
        
        public string name;
        public Stats Stats;

        [Button]
        public void AddItem()
        {
            Item item = new Item();
            item.name = name;
            //item.itemStats.AddStats(Stats);
        }
    }
}