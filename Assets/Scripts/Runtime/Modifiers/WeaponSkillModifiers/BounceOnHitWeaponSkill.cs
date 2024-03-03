using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class BounceOnHitWeaponSkill : Modifier
    {
        public int[] BounceChances = new[] { 3, 3, 3, 3 };
        public int[] BounceAmount = new[] { 1, 2, 3, 4 };

        public int tier = 0;
        public Dictionary<GameObject, int> projectiles = new Dictionary<GameObject, int>();

        public Dictionary<GameObject, int> addedAmount = new Dictionary<GameObject, int>();
        
        public BounceOnHitWeaponSkill()
        {
            SetSpecialModifier(SpecialModifiers.BounceOnHitSkill);
            SetWeaponSkill();
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            var addAmount = 3;
            if (addedAmount.ContainsKey(weapon.gameObject))
            {
                weapon.weaponStats.bounceNum -= addedAmount[weapon.gameObject];
            }
            else
            {
                addedAmount.Add(weapon.gameObject, addAmount);
            }
            
            weapon.weaponStats.bounceNum += addAmount;
        }

        public override void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            base.ApplyEffect(projectile, projectileScript, isCrit);
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
        }
    }
}