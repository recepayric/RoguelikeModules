using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.AilmentModifiers
{
    [Serializable]
    public class AddBurningEffect : Modifier
    {
        public AddBurningEffect()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            Debug.Log("Adding Burn on Hit!!");
            
            //player magic damage
            var playerBurnDamage = DictionaryHolder.Player.stats.GetStat(AllStats.BurnDamage);
            var playerBurnDamageIncrease = DictionaryHolder.Player.stats.GetStat(AllStats.BurnDamagePercentage);
            var burnDamage = 1 + playerBurnDamage;
            //burnDamage *= (1 + playerBurnDamageIncrease);
            
            //todo increaes burn time here!!!
            
            weapon.weaponStats.addBurn = true;
            weapon.weaponStats.burnTime = 3;
            weapon.weaponStats.burnDamage = burnDamage;
        }
    }
}