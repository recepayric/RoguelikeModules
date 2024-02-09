using System.Collections.Generic;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.Modifiers.OnGetDamageModifiers
{
    public class RandomCurseOnGetDamage : Modifier
    {
        private Dictionary<GameObject, AllStats> statToCurse;

        public RandomCurseOnGetDamage()
        {
            SetUseArea(ModifierUseArea.OnGetHit);
        }
        
        
        public override void ApplyEffect(Enemy enemy)
        {
            base.ApplyEffect(enemy);
            if (statToCurse == null)
                statToCurse = new Dictionary<GameObject, AllStats>();

            var contains = statToCurse.ContainsKey(enemy.gameObject);

            var statToapply = AllStats.None;
            if (contains)
                statToapply = statToCurse[enemy.gameObject];
            else
            {
                statToapply = GameConfig.randomCurses[Random.Range(0, GameConfig.randomCurses.Length)];
                statToCurse.Add(enemy.gameObject, statToapply);
            }
            
            //This one only affects player.
            //var cursable = DictionaryHolder.Cursable[DictionaryHolder.Player.gameObject];
            EventManager.Instance.AddCurse(DictionaryHolder.Player.gameObject, statToapply, 10, 1);
        }
    }
}