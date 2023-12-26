using System;

namespace Runtime.StatValue
{
    [Serializable]
    public class ElementalAilmentStatus
    {
        public bool immuneToBurn;
        public bool immuneToFreeze;
        public bool immuneToShock;
        
        public bool hitsBurn;
        public bool hitsFreeze;
        public bool hitsShock;

        public float burnDamage;
        public float freezeEffect;
        public float shockEffect;
    }
}