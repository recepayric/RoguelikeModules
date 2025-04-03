using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "ArvaveGames/Characters/Character Data", order = 0)]
    public class RD_Player : SerializedScriptableObject
    {
        public string characterName;
        public PoolKey poolKey;
        public PoolKey auraKey;
        public Dictionary<AllStats, ObscuredFloat> playerExtraStats;
        public ItemModifiers initialModifier;
    }
}