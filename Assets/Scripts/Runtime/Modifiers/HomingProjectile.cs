using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Modifiers
{
    [Serializable]
    public class HomingProjectile : Modifier
    {
        public int[] BounceChances = new[] { 30, 30, 30, 30 };
        public int[] BounceAmount = new[] { 1, 2, 3, 4 };

        public int tier = 0;
        public Dictionary<GameObject, int> projectiles = new Dictionary<GameObject, int>();

        public HomingProjectile()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Projectile projectile)
        {
            base.ApplyEffect(projectile);
            //projectile.isHomingProjectile = true;
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            weapon.weaponStats.hasHomingProjectiles = true;
        }
    }
}