using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class SphereProjectileWeaponSkill : Modifier
    {
        public Dictionary<GameObject, float> addedAmount = new Dictionary<GameObject, float>();

        public SphereProjectileWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.SphereProjectileSkill);
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;

            var damageReduction = skillData.effectPerTier[tier];

            if (addedAmount.ContainsKey(weapon.gameObject))
                weapon.weaponStats.damageReductionOnPierce -= addedAmount[weapon.gameObject];
            else
                addedAmount.Add(weapon.gameObject, 0);
                
            
            weapon.weaponStats.hasSphereProjectile = true;
            weapon.weaponStats.damageReductionOnPierce += damageReduction;
            addedAmount[weapon.gameObject] = damageReduction;
        }

        public override void RemoveEffect(Weapon weapon)
        {
            base.RemoveEffect(weapon);
            
            if (addedAmount.ContainsKey(weapon.gameObject))
            {
                weapon.weaponStats.damageReductionOnPierce -= addedAmount[weapon.gameObject];
                addedAmount.Remove(weapon.gameObject);
                weapon.weaponStats.hasSphereProjectile = false;
            }
        }
    }
}