using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Modifiers
{
    /// <summary>
    /// Splits to a certain amount of projectiles on impact
    /// Doesn't hit the same target again
    /// </summary>
    public class SplittingProjectile : Modifier
    {
        public int[] SplitAmount = new[] { 10, 4, 5, 6};

        public int tier = 0;
        
        public override void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            base.ApplyEffect(projectile, projectileScript, isCrit);

            projectileScript.split = true;
            projectileScript.splitAmount = SplitAmount[tier];
        }
    }
}