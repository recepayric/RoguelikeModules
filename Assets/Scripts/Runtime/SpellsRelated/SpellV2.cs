using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Runtime.SpellsRelated
{
    [Serializable]
    public class SpellV2
    {
        
        public int explosionNumber2 = 50;
        public float firstRadius2 = 3;
        public float radiusIncrease2 = 5f;
        public float angleIncrease2 = 2f;
        public int spiralArm2 = 2;
        
        public bool isActive;
        public float castTime = 1;
        public float timer;
        public GameObject source;

        public SpellV2()
        {
            
        }

        public virtual void ActiavateSpell()
        {
            timer = 0;
            isActive = true;
            Debug.Log("Spell Activated");
        }

        public virtual void Deactivate()
        {
            isActive = false;
        }

        public virtual void CastSpell()
        {
            Cast();
        }
        
        public virtual void Update()
        {
            if (!isActive) return;
            
            timer += Time.deltaTime;
            if (timer >= castTime)
            {
                timer = 0;
                Cast();
            }
        }

        public virtual void Cast()
        {
            
        }
    }
}