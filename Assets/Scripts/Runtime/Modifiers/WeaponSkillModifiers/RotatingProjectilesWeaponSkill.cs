using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class RotatingProjectilesWeaponSkill : Modifier
    {
        public RotatingProjectilesWeaponSkill()
        {
            SetWeaponSkill();
            SetSpecialModifier(SpecialModifiers.RotatingProjectilesSkill);
            SetUseArea(ModifierUseArea.OnStart);
        }
        
        public int tier = 0;
        public Dictionary<GameObject, int> projectiles = new Dictionary<GameObject, int>();

        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            weapon.weaponStats.hasRotatingProjectiles = true;
        }
    }
}