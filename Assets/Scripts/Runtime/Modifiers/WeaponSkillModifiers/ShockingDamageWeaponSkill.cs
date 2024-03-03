using Runtime.Enums;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class ShockingDamageWeaponSkill : Modifier
    {
        public ShockingDamageWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.ShockingDamageSkill);
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var shockEffect = skillData.effectPerTier[tier];
            var shockDuration =  skillData.secondEffectPerTier[tier];
            
            weapon.weaponStats.addShock = true;
            weapon.weaponStats.shockEffect = shockEffect;
            weapon.weaponStats.shockTime = shockDuration;
        }
    }
}