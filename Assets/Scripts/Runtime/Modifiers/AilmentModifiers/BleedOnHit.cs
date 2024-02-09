using System;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.AilmentModifiers
{
    [Serializable]
    public class BleedOnHit : Modifier
    {
        public BleedOnHit()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }
        
        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            Debug.Log("Adding Bleed on Hit!!");
            
            //player magic damage
            var playerBleedDamage = DictionaryHolder.Player.stats.GetStat(AllStats.BleedDamageIncrease);
            var playerBleedDamageIncrease = DictionaryHolder.Player.stats.GetStat(AllStats.BleedDamageMultiplier);
            var bleedDamage = 1 + playerBleedDamage;
            bleedDamage *= (1 + playerBleedDamageIncrease);
            
            //todo increaes burn time here!!!
            
            weapon.weaponStats.addBleed = true;
            weapon.weaponStats.bleedTime = 3;
            weapon.weaponStats.bleedDamage = bleedDamage;
            
        }
    }
}