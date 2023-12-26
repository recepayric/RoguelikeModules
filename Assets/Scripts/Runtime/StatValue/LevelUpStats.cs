using System;
using Runtime.Enums;

namespace Runtime.StatValue
{
    [Serializable]
    public class LevelUpStats
    {
        
        public AllStats stat;
        public float statValue;

        public LevelUpStats(AllStats selectedStat, float statValue)
        {
            stat = selectedStat;
            this.statValue = statValue;
        }
    }
}