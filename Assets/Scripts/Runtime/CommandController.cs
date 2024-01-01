using System;
using Runtime.Enums;
using Runtime.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime
{
    public class CommandController : MonoBehaviour
    {
        public static CommandController instance;
        private void Awake()
        {
            instance = this;
        }


        public void SplitProjectile(Vector3 centerPosition, int splitAmount, PoolKeys projectileKey, IDamageable ignoredEnemy)
        {
            for (int i = 0; i < splitAmount; i++)
            {
                var projectile = BasicPool.instance.Get(projectileKey);
                projectile.transform.position = centerPosition;
                projectile.transform.rotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
                //projectile.transform.rotation = Quaternion.Euler(0, 0, 0);
                var sc = ScriptDictionaryHolder.Projectiles[projectile];
                sc.ignoredEnemy = ignoredEnemy;
                //sc.bounceNum = 1;
                //sc.pierceNum = 1;
                //sc.criticalHitChance = weaponStats.criticalHitChance / 100f;
                //sc.criticalHitDamage = weaponStats.criticalHitDamage;
                //sc.modifiers = modifiers;
            }
        }
    }
}