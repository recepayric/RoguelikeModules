using System.Collections.Generic;
using Data;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class ProjectileExplodeOnXAttackWeaponSkill : Modifier
    {
        public ProjectileExplodeOnXAttackWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.ProjectileExplodeOnXAttackSkill);
            SetUseArea(ModifierUseArea.OnBeforeHit);
            GetSkillData();
        }
        
        public int tier = 0;
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            Debug.Log(skillData + "    ");
            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var attackNumToExplode = skillData.effectPerTier[tier];
            var explosionDamageMult =  skillData.secondEffectPerTier[tier];

            Debug.Log("Checking for explosion!!");
            if (weapon.weaponStats.totalAttackNumber % attackNumToExplode != 0)
            {
                Debug.Log("No Explosion!");

                weapon.weaponStats.explodingProjectile = false;
                return;
            }
            Debug.Log("Yes Explosion!");

            weapon.weaponStats.explodingProjectile = true;
            weapon.weaponStats.explosionDamageMultiplier = explosionDamageMult;
        }
    }
}