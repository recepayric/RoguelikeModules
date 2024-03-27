using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.PlayerRelated;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.SpellsRelated
{
    public class GambleStatSpell : Spell
    {
        public Player playerScript;
        public List<AllStats> statsToGambleOn;
        public int[] statIncreases = new[] { -30, -20, -10, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100 };

        public override void Cast()
        {
            var randomStat = statsToGambleOn[Random.Range(0, statsToGambleOn.Count)];
            var increase = statIncreases[Random.Range(0, statIncreases.Length)];
            Debug.Log(randomStat + " increases by %" + increase);
            OwnerScript.AddGambleStat(randomStat, increase);
        }

        public override void Activate()
        {
        }

        public override void DeActivate()
        {
            base.DeActivate();
        }

        public override void Prepare()
        {
        }

        public override void StartSpell()
        {
            base.StartSpell();
            Cast();
        }

        public override void StopSpell()
        {
            base.StopSpell();
            DeActivate();
        }

        public override void SetPosition(Vector3 targetPosition)
        {
        }

        public void Update()
        {
        }
    }
}