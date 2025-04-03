using System;
using Runtime.Enums;

namespace Runtime.Data.Structs
{
    [Serializable]
    public struct AttackPatternData
    {
        public float attackPercentage;
        public PoolKey projectileKey;
        public PoolKey muzzleKey;
        public int attackCount;
        public int projectileCount;
        public float attackTimeMultiplier;
        public bool isContinuous;
        public bool turnToEnemyDuringShooting;
        public bool enableManualAttackEnd;
    }
}