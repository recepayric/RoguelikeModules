using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/WeaponDataObject", order = 1)]
    public class WeaponDatasSo : SerializedScriptableObject
    {
        public Dictionary<Weapons, WeaponData> WeaponData;
        public List<SpecialModifiers> specialModifiersList;
    }

    public class WeaponData
    {
        public string WeaponName;
        public string Description;
        public WeaponTypes WeaponType;
        public float BaseAttackSpeed;
        public float BaseAttackRange;
        public float BaseDamage;
        public int BaseProjectileAmount;
        public int BaseCriticalHitChance;
        public int BaseCriticalHitDamage;
        public int BasePierceNumber;
        public PoolKeys WeaponPoolKey;
    }
}

