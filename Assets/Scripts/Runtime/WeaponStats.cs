using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Modifiers;
using Sirenix.OdinInspector;
using UnityEngine;

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

        public int bounceNum;
        public int pierceNum;

        public List<SpecialModifiers> specialModifiers;

        //Elemental Values
        [HideInInspector] public bool addBurn;
        [HideInInspector] public bool addFreeze;
        [HideInInspector] public bool addShock;

        [HideInInspector] public float burnTime;
        [HideInInspector] public float burnDamage;

        [HideInInspector] public float freezeTime;
        [HideInInspector] public float freezeEffect;

        [HideInInspector] public float shockTime;
        [HideInInspector] public float shockEffect;
    }
}