using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class BleedChanceOnHitWeaponSkill : Modifier
    {
        public Dictionary<GameObject, float> addedAmount = new Dictionary<GameObject, float>();
        
        public BleedChanceOnHitWeaponSkill()
        {
            SetSpecialModifier(SpecialModifiers.BleedChanceOnHitSkill);
            SetWeaponSkill();
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var bleedChance = skillData.effectPerTier[tier];

            //todo save added chances and stuff!!!
            var weaponDamage = weapon.GetDamage();
            var bleedDuration = skillData.thirdEffectPerTier[tier]*weaponDamage/100f;
            var bleedDamage = skillData.secondEffectPerTier[tier];
            
            if (!addedAmount.ContainsKey(weapon.gameObject))
            {
                addedAmount.Add(weapon.gameObject, bleedChance);
            }
            else
            {
                weapon.weaponStats.bleedChance -= addedAmount[weapon.gameObject];
            }
            
            //var burnDuration =  skillData.secondEffectPerTier[tier];
            weapon.weaponStats.bleedChance += bleedChance;
            weapon.weaponStats.bleedDamage = bleedDamage;
            weapon.weaponStats.bleedTime = bleedDuration;
        }
        
        public override void RemoveEffect(Weapon weapon)
        {
            base.RemoveEffect(weapon);

            if (addedAmount.ContainsKey(weapon.gameObject))
            {
                weapon.weaponStats.bleedChance -= addedAmount[weapon.gameObject];
                addedAmount.Remove(weapon.gameObject);
            }
        }
    }
}