using System;
using Runtime.Enums;

namespace Runtime.Misc
{
    [Serializable]
    public class StatDetailed
    {
        public int tierNumber;
        public AllStats stat;
        public int minValue;
        public int maxValue;
        public int currentValue;
    }
}