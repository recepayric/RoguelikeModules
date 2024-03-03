using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.Modifiers.AilmentModifiers;
using Runtime.Modifiers.CharacterModifiers;
using Runtime.Modifiers.OnGetDamageModifiers;
using Runtime.Modifiers.ProjectileModifiers;
using Runtime.Modifiers.WeaponSkillModifiers;
using UnityEngine;

namespace Runtime.Modifiers
{
    public static class ModifierCreator
    {
        private static Dictionary<SpecialModifiers, Modifier> allModifiers;
        public static WeaponSkillDataSo weaponSkillDataSo;

        private static void AddModifiers()
        {
            if (allModifiers == null) allModifiers = new Dictionary<SpecialModifiers, Modifier>();
            
            weaponSkillDataSo = Resources.Load<WeaponSkillDataSo>("WeaponSkillData");
            
            allModifiers.Add(SpecialModifiers.BounceOnCriticalStrike, new BounceOnCrit());
            allModifiers.Add(SpecialModifiers.SplitOnHit, new SplitOnHit());
            allModifiers.Add(SpecialModifiers.RotatingProjectiles, new RotatingProjectiles());
            allModifiers.Add(SpecialModifiers.HomingProjectiles, new HomingProjectile());
            allModifiers.Add(SpecialModifiers.BurnOnHit, new AddBurningEffect());
            allModifiers.Add(SpecialModifiers.FreezeOnHit, new AddFreezingOnHit());
            allModifiers.Add(SpecialModifiers.ShockOnHit, new AddShockOnHit());
            allModifiers.Add(SpecialModifiers.LowHealthMoreAttackSpeed, new LowHealthMoreAttackSpeed());
            allModifiers.Add(SpecialModifiers.BleedOnHit, new BleedOnHit());
            allModifiers.Add(SpecialModifiers.StunChanceOnHit, new StunChanceOnHit());
            allModifiers.Add(SpecialModifiers.CurseRandomOnGetHit, new RandomCurseOnGetDamage());
            allModifiers.Add(SpecialModifiers.DealNoBurn, new DealNoBurn());
            allModifiers.Add(SpecialModifiers.DealNoFreeze, new DealNoFreeze());
            allModifiers.Add(SpecialModifiers.DealNoShock, new DealNoShock());
            allModifiers.Add(SpecialModifiers.HealthModifierReduction, new HealthModifierReduction());
            
            
            //Weapon Skills
            allModifiers.Add(SpecialModifiers.BounceOnHitSkill, new BounceOnHitWeaponSkill());
            allModifiers.Add(SpecialModifiers.RotatingProjectilesSkill, new RotatingProjectilesWeaponSkill());
            allModifiers.Add(SpecialModifiers.ProjectileExplodeOnXAttackSkill, new ProjectileExplodeOnXAttackWeaponSkill());
            allModifiers.Add(SpecialModifiers.BurningDamageSkill, new BurningDamageWeaponSkill());
            allModifiers.Add(SpecialModifiers.FreezingDamageSkill, new FreezingDamageWeaponSkill());
            allModifiers.Add(SpecialModifiers.ShockingDamageSkill, new ShockingDamageWeaponSkill());
            allModifiers.Add(SpecialModifiers.HomingProjectilesSkill, new HomingProjectilesWeaponSkill());
            allModifiers.Add(SpecialModifiers.SphereProjectileSkill, new SphereProjectileWeaponSkill());
            allModifiers.Add(SpecialModifiers.MeteorSpawnSkill, new MeteorFallWeaponSkill());
        }

        public static Modifier GetModifier(SpecialModifiers specialModifiers)
        {
            if (allModifiers == null) AddModifiers();

            if (!allModifiers.ContainsKey(specialModifiers))
            {
                Debug.LogError("Special Modifier " + specialModifiers.ToString() + " is not registered!");
                return null;
            }
            return allModifiers[specialModifiers];
        }
    }
}