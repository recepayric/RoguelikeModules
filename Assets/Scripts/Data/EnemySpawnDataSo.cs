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

        public int totalMonster;
        public float initialTimeBetweenEnemies;
        public int minEnemySpawn;
        public int maxEnemySpawn;
        //public int waveInRound;
        //public int enemyPerWave;
        public float totalSpawnTime;
        public int round;
        public float spawnSpeedPerRound = 0.1f;
        

        [Button]
        public void AddRandomEnemies()
        {
            if (SpawnDatas == null)
                SpawnDatas = new List<SpawnData>();
            SpawnDatas.Clear();
            totalMonster = 0;
            totalSpawnTime = 0;
            round = 0;

            do
            {
                var enemyToAdd = Random.Range(minEnemySpawn, maxEnemySpawn);
                var timeBetween = 3 - round*spawnSpeedPerRound;

                totalMonster += enemyToAdd;
                if (totalSpawnTime + timeBetween > 60)
                    timeBetween = 60 - totalSpawnTime;
                
                totalSpawnTime += timeBetween;
                round++;
                
                SpawnData spawnData = new SpawnData();
                spawnData.Amount = enemyToAdd;
                spawnData.EnemyKey = poolKeyArray[Random.Range(0, poolKeyArray.Length)];
                spawnData.spawnWait = timeBetween;
                SpawnDatas.Add(spawnData);
                
                if(round > 100)
                    break;
            } while (totalSpawnTime < 60);
        }
    }

    [Serializable]
    public class SpawnData
    {
        public PoolKeys EnemyKey;
        public int Amount;
        public float spawnWait;
    }
}