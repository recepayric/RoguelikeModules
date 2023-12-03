using System;
using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Modifiers.ProjectileModifiers
{
    [Serializable]
    public class RotatingProjectiles : Modifier
    {
        public int tier = 0;
        public Dictionary<GameObject, int> projectiles = new Dictionary<GameObject, int>();

        public override void ApplyEffect(Projectile projectile)
        {
            base.ApplyEffect(projectile);
        }
    }
}