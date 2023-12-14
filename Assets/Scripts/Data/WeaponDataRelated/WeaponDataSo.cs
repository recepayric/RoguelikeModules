using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.WeaponDataRelated
{
    [CreateAssetMenu(fileName = "Data", menuName = "Weapons/WeaponDataObject", order = 1)]
    public class WeaponDataSo : SerializedScriptableObject
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
        public WeaponData WeaponData;
    }
}

