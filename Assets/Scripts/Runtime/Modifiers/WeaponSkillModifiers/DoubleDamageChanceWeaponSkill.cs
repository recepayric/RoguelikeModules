using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class DoubleDamageChanceWeaponSkill : Modifier
    {
        public Dictionary<GameObject, float> addedEffect = new Dictionary<GameObject, float>();
        
        public DoubleDamageChanceWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.ProjectileExplodeOnXAttackSkill);
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var doubleDamageChance = skillData.effectPerTier[tier];

            if (addedEffect.ContainsKey(weapon.gameObject))
            {
                weapon.weaponStats.doubleDamageChance -= addedEffect[weapon.gameObject];
            }
            else
            {
                addedEffect.Add(weapon.gameObject, doubleDamageChance);
            }
            
            
            weapon.weaponStats.doubleDamageChance += doubleDamageChance;
        }

        public override void RemoveEffect(Weapon weapon)
        {
            base.RemoveEffect(weapon);
            if (addedEffect.ContainsKey(weapon.gameObject))
            {
                weapon.weaponStats.doubleDamageChance -= addedEffect[weapon.gameObject];
                addedEffect.Remove(weapon.gameObject);
            }
        }
    }
}