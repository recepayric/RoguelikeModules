using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.ItemsRelated
{
    [Serializable]
    public class Item
    {
        public string name;
        public Stats itemStats;
        public ItemRarity rarity;
        public int quantity;
    }
}