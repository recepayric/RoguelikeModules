using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.ItemsRelated
{
    [Serializable]
    public class Item
    {
        public Sprite itemIcon;
        public string name;
        public string description;
        //public Stats itemStats;
        public Dictionary<AllStats, float> stats;
        public List<AllStats> statNames;
        public List<float> statValues;
        public ItemRarity rarity;
        public int quantity;

    }
}