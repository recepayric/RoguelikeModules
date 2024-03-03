using Runtime.Enums;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class FreezingDamageWeaponSkill : Modifier
    {
        public FreezingDamageWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.FreezingDamageSkill);
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;
            
            var freezeEffect = skillData.effectPerTier[tier];
            var freezeTime =  skillData.secondEffectPerTier[tier];
            
            weapon.weaponStats.addFreeze = true;
            weapon.weaponStats.freezeEffect = freezeEffect;
            weapon.weaponStats.freezeTime = freezeTime;
        }
    }
}