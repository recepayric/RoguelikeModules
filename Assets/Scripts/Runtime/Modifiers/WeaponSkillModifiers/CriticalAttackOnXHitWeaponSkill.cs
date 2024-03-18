using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class CriticalAttackOnXHitWeaponSkill : Modifier
    {
        public CriticalAttackOnXHitWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.CriticalHitOnXHitSkill);
            SetUseArea(ModifierUseArea.OnHit);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            Debug.Log(skillData + "    ");
            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var attackNumToCrit = skillData.effectPerTier[tier];

            if (weapon.weaponStats.totalAttackNumber % attackNumToCrit != 0)
            {
                weapon.weaponStats.criticalHitForSure = false;
                return;
            }
            
            weapon.weaponStats.criticalHitForSure = true;
        }
    }
}