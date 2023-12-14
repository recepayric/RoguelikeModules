using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.ElementalModifiers
{
    [Serializable]
    public class AddFreezingOnHit : Modifier
    { 
        public AddFreezingOnHit()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            
            //player magic damage
            var playerFreezeEffect = ScriptDictionaryHolder.Player.stats.GetStat(AllStats.FreezingEffect);;
            var freezeEffect = 10 + playerFreezeEffect;
            
            //todo increaes burn time here!!!
            
            weapon.weaponStats.addFreeze = true;
            weapon.weaponStats.freezeTime = 1;
            weapon.weaponStats.freezeEffect = freezeEffect;
        }
    }
}