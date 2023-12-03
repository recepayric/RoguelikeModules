using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Modifiers.CharacterModifiers;
using Runtime.Modifiers.ElementalModifiers;
using UnityEngine;

namespace Runtime.Modifiers
{
    public static class ModifierCreator
    {
        private static Dictionary<SpecialModifiers, Modifier> allModifiers;

        public static Modifier GetModifier(SpecialModifiers specialModifiers)
        {
            if (allModifiers == null) allModifiers = new Dictionary<SpecialModifiers, Modifier>();

            if (allModifiers.ContainsKey(specialModifiers))
            {
                return allModifiers[specialModifiers];
            }

            switch (specialModifiers)
            {
                case SpecialModifiers.BounceOnCriticalStrike:
                    BounceOnCrit bounceOnCrit = new BounceOnCrit();
                    allModifiers.Add(specialModifiers, bounceOnCrit);
                    return bounceOnCrit;

                case SpecialModifiers.SplitOnHit:
                    SplittingProjectile splittingProjectile = new SplittingProjectile();
                    allModifiers.Add(specialModifiers, splittingProjectile);
                    return splittingProjectile;

                case SpecialModifiers.RotatingProjectiles:
                    
                case SpecialModifiers.HomingProjectiles:
                    
                case SpecialModifiers.BurnOnHit:
                    AddBurningEffect addBurningEffect = new AddBurningEffect();
                    allModifiers.Add(specialModifiers, addBurningEffect);
                    return addBurningEffect;
                
                case SpecialModifiers.FreezeOnHit:
                    AddFreezingOnHit addFreezingOnHit = new AddFreezingOnHit();
                    allModifiers.Add(specialModifiers, addFreezingOnHit);
                    return addFreezingOnHit;
                
                case SpecialModifiers.ShockOnHit:
                    AddShockOnHit addShockOnHit = new AddShockOnHit();
                    allModifiers.Add(specialModifiers, addShockOnHit);
                    return addShockOnHit;

                case SpecialModifiers.LowHealthMoreAttackSpeed:
                    LowHealthMoreAttackSpeed lowHealthMoreAttackSpeed = new LowHealthMoreAttackSpeed();
                    allModifiers.Add(specialModifiers, lowHealthMoreAttackSpeed);
                    return lowHealthMoreAttackSpeed;
                    
                default:
                    throw new ArgumentOutOfRangeException(nameof(specialModifiers), specialModifiers, null);
            }
            
            return null;
        }
    }
}