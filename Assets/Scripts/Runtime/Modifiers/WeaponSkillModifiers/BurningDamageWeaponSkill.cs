using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class BurningDamageWeaponSkill : Modifier
    {
        public BurningDamageWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.BurningDamageSkill);
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var burnDamage = skillData.effectPerTier[tier];
            var burnDuration =  skillData.secondEffectPerTier[tier];
            
            weapon.weaponStats.addBurn = true;
            weapon.weaponStats.burnDamage = burnDamage;
            weapon.weaponStats.burnTime = burnDuration;
        }
    }
}