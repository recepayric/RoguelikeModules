using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.CharacterModifiers
{
    public class LowHealthMoreAttackSpeed : Modifier
    {
        public Dictionary<GameObject, float> addedAttackSpeeds;

        public override void ApplyEffect(Player player)
        {
            base.ApplyEffect(player);

            if (addedAttackSpeeds == null)
                addedAttackSpeeds = new Dictionary<GameObject, float>();
            
            var value = 0f;
            if (addedAttackSpeeds.ContainsKey(player.gameObject))
                value = addedAttackSpeeds[player.gameObject];
            else
                addedAttackSpeeds.Add(player.gameObject, 0f);
            
            //todo maybe multiplative???
            player.stats.IncreaseStat(AllStats.AttackSpeed, -value);

            var missingHealth = (player.damageTaken / player.maxHealth) * 100f;
            value = missingHealth;

            addedAttackSpeeds[player.gameObject] = missingHealth;
            player.stats.IncreaseStat(AllStats.AttackSpeed, value);
            player.stats.SetStatValues();
        }
    }
}