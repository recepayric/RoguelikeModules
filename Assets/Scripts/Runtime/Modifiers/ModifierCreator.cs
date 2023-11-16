using System;
using System.Collections.Generic;
using Runtime.Enums;
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(specialModifiers), specialModifiers, null);
            }


            return null;
        }
    }
}