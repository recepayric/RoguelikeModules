using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers
{
    [Serializable]
    public class Modifier
    {
        public ModifierUseArea useArea;
        private Dictionary<GameObject, int> amountOfModifier;

        public void SetUseArea(ModifierUseArea modifierUseArea)
        {
            useArea = modifierUseArea;
        }

        public virtual void ApplyEffect()
        {
        }

        public virtual void ApplyEffect(Player player)
        {
            //RegisterUser(player.gameObject);
        }

        public virtual void ApplyEffect(Projectile projectile)
        {
            //RegisterUser(projectile.gameObject);
        }

        public virtual void ApplyEffect(Weapon weapon)
        {
            //RegisterUser(weapon.gameObject);
        }

        public virtual void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            //RegisterUser(projectile);
        }

        public virtual void RegisterUser(GameObject gameObject)
        {
            if (amountOfModifier == null)
                amountOfModifier = new Dictionary<GameObject, int>();

            if (!amountOfModifier.ContainsKey(gameObject))
                amountOfModifier.Add(gameObject, 0);

            amountOfModifier[gameObject]++;
        }

        public void RemoveRegisteredUser(GameObject gameObject)
        {
            if (amountOfModifier.ContainsKey(gameObject))
                amountOfModifier.Remove(gameObject);
        }
        
    }
}