using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.ElementalModifiers
{
    [Serializable]
    public class AddBurningEffect : Modifier
    { 
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            
            //player magic damage
            var playerBurnDamage = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.BurnDamage);
            var playerBurnDamageIncrease = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.BurnDamagePercentage);
            var burnDamage = 1 + playerBurnDamage;
            burnDamage *= (1 + playerBurnDamageIncrease);
            
            //todo increaes burn time here!!!
            
            weapon.weaponStats.addBurn = true;
            weapon.weaponStats.burnTime = 3;
            weapon.weaponStats.burnDamage = burnDamage;
        }
    }
}