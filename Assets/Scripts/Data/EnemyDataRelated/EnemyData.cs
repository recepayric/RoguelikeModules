using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.EnemyDataRelated
{
    [CreateAssetMenu(fileName = "Data", menuName = "Enemies/EnemyData", order = 1)]
    public class EnemyData : SerializedScriptableObject
    {
        public bool canBeSpawned;
        public bool CanBeSpawnedNormally;
        public float baseHealth;
        public float baseDamage;
        public float baseMoveSpeed;
        public float baseAttackSpeed;
        public float baseEvasion;
        public float baseDefence;
        public float baseAttackRange;
        public float baseMaxAttackRange;
        public float baseProjectileNumber;
        public int minFloorToSpawn;
        public int minSpawnAmount;
        public int maxSpawnAmount;
        public AttackType attackType;

        [ShowIf("attackType", AttackType.Bomber)]
        public PoolKeys explosionKey;
        public PoolKeys poolKey;
    }
}