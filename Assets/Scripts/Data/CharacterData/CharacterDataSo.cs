using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Character/Character Data", order = 1)]
    public class CharacterDataSo : SerializedScriptableObject
    {
        public Dictionary<AllStats, float> BaseStats;
        public List<SpecialModifiers> specialModifiers;
    }
}