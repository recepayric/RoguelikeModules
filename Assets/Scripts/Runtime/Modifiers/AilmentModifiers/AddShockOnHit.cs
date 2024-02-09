using System;
using Runtime.Enums;

namespace Runtime.Modifiers.AilmentModifiers
{
    [Serializable]
    public class AddShockOnHit : Modifier
    { 
        
        public AddShockOnHit()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            //player magic damage
            var playerShockEffect = DictionaryHolder.Player.stats.GetStat(AllStats.ShockEffect);;
            var freezeEffect = 10 + playerShockEffect;
            
            //todo increaes burn time here!!!
            
            weapon.weaponStats.addShock = true;
            weapon.weaponStats.shockTime = 3;
            weapon.weaponStats.shockEffect = playerShockEffect;
        }
    }
}