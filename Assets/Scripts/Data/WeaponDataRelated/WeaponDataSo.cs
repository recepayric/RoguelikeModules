using System.Collections.Generic;
using Runtime.Enums;
using Runtime.SpellsRelated;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.WeaponDataRelated
{
    [CreateAssetMenu(fileName = "Data", menuName = "Weapons/WeaponDataObject", order = 1)]
    public class WeaponDataSo : SerializedScriptableObject
    {
        public string WeaponName;
        public string Description;
        public bool isStarterWeapon;
        public WeaponType WeaponType;
        public float BaseAttackSpeed;
        public float BaseAttackRange;
        public float BaseAttackRangeSword;
        public float BaseDamage;
        public int BaseProjectileAmount;
        public int BaseCriticalHitChance;
        public int BaseCriticalHitDamage;
        public int BasePierceNumber;
        public int BaseBounceNumber;
        public int rotationDistanceFromPlayer;
        public List<SpecialModifiers> specialModifiersList;
        public List<Spells> spells;
        public PoolKeys WeaponPoolKey;
        public PoolKeys DummyWeaponKey;
        public PoolKeys RotatingWeaponKey;
        public WeaponData WeaponData;
        public Sprite waaponSprite;
        public Sounds attackSound;
    }
}

