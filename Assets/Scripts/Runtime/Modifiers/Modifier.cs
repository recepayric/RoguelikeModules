using System;
using UnityEngine;

namespace Runtime.Modifiers
{
    [Serializable]
    public class Modifier
    {
        public virtual void ApplyEffect()
        {
            
        }

        public virtual void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
        }
    }
}