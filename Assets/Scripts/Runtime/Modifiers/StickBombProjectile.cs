using UnityEngine;

namespace Runtime.Modifiers
{
    public class StickBombProjectile : Modifier
    {
        public override void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            base.ApplyEffect(projectile, projectileScript, isCrit);
        }
    }
}