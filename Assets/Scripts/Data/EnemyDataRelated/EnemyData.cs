using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data.EnemyDataRelated
{
    [CreateAssetMenu(fileName = "Data", menuName = "Enemies/EnemyData", order = 1)]
    public class EnemyData : SerializedScriptableObject
    {
        public float baseHealth;
        public float baseDamage;
        public float baseMoveSpeed;
        public float baseAttackSpeed;
        public float baseEvasion;
        public float baseDefence;
        public float baseAttackRange;
        public float baseMaxAttackRange;
        public float baseProjectileNumber;
        public AttackType attackType;
    }
}