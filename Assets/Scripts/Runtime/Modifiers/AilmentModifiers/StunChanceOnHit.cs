using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.AilmentModifiers
{
    public class StunChanceOnHit : Modifier
    {
        public StunChanceOnHit()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            var stunTime = 3;
            
            //todo increaes burn time here!!!
            
            weapon.weaponStats.addStun = true;
            weapon.weaponStats.stunChance = 10f;
            weapon.weaponStats.stunTime = 1f;
        }
    }
}