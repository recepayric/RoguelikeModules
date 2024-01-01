using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Modifiers.CharacterModifiers;
using Runtime.Modifiers.ElementalModifiers;
using Runtime.Modifiers.ProjectileModifiers;
using UnityEngine;

namespace Runtime.Modifiers
{
    public static class ModifierCreator
    {
        private static Dictionary<SpecialModifiers, Modifier> allModifiers;

        private static void AddModifiers()
        {
            if (allModifiers == null) allModifiers = new Dictionary<SpecialModifiers, Modifier>();
            
            
            allModifiers.Add(SpecialModifiers.BounceOnCriticalStrike, new BounceOnCrit());
            allModifiers.Add(SpecialModifiers.SplitOnHit, new SplitOnHit());
            allModifiers.Add(SpecialModifiers.RotatingProjectiles, new RotatingProjectiles());
            allModifiers.Add(SpecialModifiers.HomingProjectiles, new HomingProjectile());
            allModifiers.Add(SpecialModifiers.BurnOnHit, new AddBurningEffect());
            allModifiers.Add(SpecialModifiers.FreezeOnHit, new AddFreezingOnHit());
            allModifiers.Add(SpecialModifiers.ShockOnHit, new AddShockOnHit());
            allModifiers.Add(SpecialModifiers.LowHealthMoreAttackSpeed, new LowHealthMoreAttackSpeed());
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