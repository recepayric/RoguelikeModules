using System.Collections.Generic;
using Runtime.Enums;
using Runtime.SpellsRelated;
using Runtime.SpellsRelated.Cast;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class MeteorFallWeaponSkill : Modifier
    {
        public Dictionary<GameObject, SpellV2> addedAmount = new Dictionary<GameObject, SpellV2>();

        public MeteorFallWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.MeteorSpawnSkill);
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;

            var castTime = skillData.effectPerTier[tier];
            if (addedAmount.ContainsKey(weapon.gameObject)) return;

            var spell = new MeteorFallSpell();
            spell.castTime = castTime;
            
            weapon.weaponStats.spells.Add(spell);
            addedAmount.Add(weapon.gameObject, spell);
        }

        public override void RemoveEffect(Weapon weapon)
        {
            base.RemoveEffect(weapon);
            
            if (addedAmount.ContainsKey(weapon.gameObject))
            {
                var spell = addedAmount[weapon.gameObject];
                spell.isActive = false;
                weapon.weaponStats.spells.Remove(spell);
                addedAmount.Remove(weapon.gameObject);
            }
        }
    }
}