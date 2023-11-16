using System;
using System.Collections.Generic;
using Runtime;
using Runtime.Enums;
using Runtime.WorldRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using Item = Runtime.ItemsRelated.Item;
using Random = UnityEngine.Random;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Spawn/EnemySpawnData", order = 0)]
    public class EnemySpawnDataSo : SerializedScriptableObject
    {
        public List<SpawnData> SpawnDatas;

        public PoolKeys[] poolKeyArray;

        public int waveInRound;
        public int enemyPerWave;

        [Button]
        public void AddRandomEnemies()
        {
            if (SpawnDatas == null)
                SpawnDatas = new List<SpawnData>();
            SpawnDatas.Clear();

            for (int i = 0; i < waveInRound; i++)
            {
                Debug.Log("Adding a random enemy");
                SpawnData spawnData = new SpawnData();
                spawnData.Amount = enemyPerWave;
                spawnData.EnemyKey = poolKeyArray[Random.Range(0, poolKeyArray.Length)];
                SpawnDatas.Add(spawnData);
            }
        }
    }

    [Serializable]
    public class SpawnData
    {
        public PoolKeys EnemyKey;
        public int Amount;
    }
}