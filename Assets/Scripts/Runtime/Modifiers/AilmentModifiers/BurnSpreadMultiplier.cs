using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.AilmentModifiers
{
    public class BurnSpreadMultiplier : Modifier
    {
        public Dictionary<GameObject, int> addedMultiplier;
        
        public BurnSpreadMultiplier()
        {
            SetUseArea(ModifierUseArea.OnStart);
        }

        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);
            if (addedMultiplier == null)
                addedMultiplier = new Dictionary<GameObject, int>();

            if(addedMultiplier.ContainsKey(weapon.gameObject))
                weapon.weaponStats.burnSpreadMultiplier -= addedMultiplier[weapon.gameObject];
            
            var total = amountOfModifier[weapon.gameObject];
            weapon.weaponStats.burnSpreadMultiplier += total;

            if (addedMultiplier.ContainsKey(weapon.gameObject))
                addedMultiplier[weapon.gameObject] = total;
            else
                addedMultiplier.Add(weapon.gameObject, total);
        }
    }
}