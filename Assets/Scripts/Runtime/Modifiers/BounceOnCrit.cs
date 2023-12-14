using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.Modifiers
{
    [Serializable]
    public class BounceOnCrit : Modifier
    {
        public int[] BounceChances = new[] { 30, 30, 30, 30 };
        public int[] BounceAmount = new[] { 1, 2, 3, 4 };

        public int tier = 0;
        public Dictionary<GameObject, int> projectiles = new Dictionary<GameObject, int>();
        
        
        public BounceOnCrit()
        {
            SetUseArea(ModifierUseArea.OnHit);
        }
        
        
        public override void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            base.ApplyEffect(projectile, projectileScript, isCrit);

            if(projectileScript.bounceNum > 0) return;

            if (!isCrit)
            {
                if (projectiles.ContainsKey(projectile))
                    projectiles.Remove(projectile);
                
                return;
            }


            if (!projectiles.ContainsKey(projectile))
                projectiles.Add(projectile, BounceAmount[tier]);
            
            if(projectiles[projectile] == 0 ) return;

            var perc = Random.Range(0, 100f);
            
            if (perc <= BounceChances[tier])
            {
                projectileScript.bounceNum++;
                projectiles[projectile]--;
            }
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
        }
    }
}