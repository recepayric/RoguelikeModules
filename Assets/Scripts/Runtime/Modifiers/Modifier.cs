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
        
        public virtual void ApplyEffect(Player player){}
        public virtual void ApplyEffect(Projectile projectile){}
        public virtual void ApplyEffect(Weapon weapon){}

        public virtual void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
        }
    }
}