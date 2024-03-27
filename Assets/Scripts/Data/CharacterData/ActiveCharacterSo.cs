using System.Collections.Generic;
using Runtime;
using Runtime.Enums;
using Runtime.ItemsRelated;
using Runtime.PlayerRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Character/ActiveCharacterData", order = 1)]
    public class ActiveCharacterSo : SerializedScriptableObject
    {
        public GameObject playerObject;
        public Player playerScript;

        public List<Weapon> playerWeapons;
        public List<Item> playerItems;
    }
}