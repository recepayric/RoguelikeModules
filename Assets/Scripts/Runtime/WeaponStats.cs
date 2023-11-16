using System;
using Runtime.Enums;

namespace Runtime
{
    [Serializable]
    public class WeaponStats
    {
        public WeaponTypes weaponType;
        public float attackSpeed;
        public float damage;
        public float range;
        public int projectileAmount;
        public int criticalHitChance;
        public int criticalHitDamage;
    }
}