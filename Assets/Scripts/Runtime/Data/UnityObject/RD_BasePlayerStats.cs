using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "ArvaveGames/Characters/Base Character Data", order = 0)]
    public class RD_BasePlayerStats : SerializedScriptableObject
    {
        public Dictionary<AllStats, ObscuredFloat> playerBaseStats;
    }
}