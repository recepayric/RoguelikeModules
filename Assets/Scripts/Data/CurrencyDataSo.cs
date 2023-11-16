using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CollectableData", order = 1)]
    public class CurrencyDataSo : SerializedScriptableObject
    {
        public Dictionary<CollectableTypes, int> collectables = new Dictionary<CollectableTypes, int>();

        public void AddCollectable(CollectableTypes type, int amount)
        {
            if (!collectables.ContainsKey(type))
                collectables.Add(type, 0);
            
            collectables[type] += amount;
        }

        public int GetCollectableAmount(CollectableTypes type)
        {
            return collectables.ContainsKey(type) ? collectables[type] : 0;
        }
    }
}