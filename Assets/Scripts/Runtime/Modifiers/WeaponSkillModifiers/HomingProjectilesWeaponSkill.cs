using Runtime.Enums;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class HomingProjectilesWeaponSkill : Modifier
    {
        public HomingProjectilesWeaponSkill()
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
            
            weapon.weaponStats.hasHomingProjectiles = true;
        }
    }
}