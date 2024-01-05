using System;
using System.Collections.Generic;
using System.Linq;
using Data.EnemyDataRelated;
using Runtime;
using Runtime.Enums;
using Runtime.WorldRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using Item = Runtime.ItemsRelated.Item;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Spawn/EnemySpawnData", order = 0)]
    public class EnemySpawnDataSo : SerializedScriptableObject
    {
        public float floorTime;
        public Object[] enemyDataObjects;
        public EnemyData[] enemyDatas;
        public List<SpawnData> SpawnDatas;

        //public PoolKeys[] poolKeyArray;
        public List<PoolKeys> poolKeys;
        public List<PoolKeys> bufferKeys;

        public int totalMonster;
        public float initialTimeBetweenEnemies;
        public int minEnemySpawn;
        public int maxEnemySpawn;

        //public int waveInRound;
        //public int enemyPerWave;
        public float totalSpawnTime;
        public int floor;
        public int round;
        public float spawnSpeedPerRound = 0.1f;
        public float BufferSpawnPercentage;

        [Button]
        public void GetAllResources()
        {
            enemyDataObjects = Resources.LoadAll("EnemyDatas", typeof(EnemyData));
            enemyDatas = new EnemyData[enemyDataObjects.Length];
            for (int i = 0; i < enemyDataObjects.Length; i++)
            {
                enemyDatas[i] = (EnemyData)enemyDataObjects[i];
            }
        }


        private void SetPoolKeys()
        {
            poolKeys.Clear();
            for (int i = 0; i < enemyDatas.Length; i++)
            {
                //ignore aura users here!!
                if (enemyDatas[i].attackType == AttackType.AuraUser) continue;

                if (floor >= enemyDatas[i].minFloorToSpawn)
                {
                    poolKeys.Add(enemyDatas[i].poolKey);
                }
            }
        }

        private void CheckBuffers()
        {
            var buffers = enemyDatas.ToList().FindAll(t => t.attackType == AttackType.AuraUser);

            bufferKeys.Clear();

            for (int i = 0; i < buffers.Count; i++)
            {
                if (floor >= buffers[i].minFloorToSpawn)
                {
                    bufferKeys.Add(buffers[i].poolKey);
                }
            }
        }

        private void SpawnBuffer(SpawnData spawnData)
        {
            if (bufferKeys.Count == 0) return;
            if (Random.Range(0, 1f) > BufferSpawnPercentage) return;
            var bufferToSpawn = bufferKeys[Random.Range(0, bufferKeys.Count)];
            var min = 0;
            var max = 0;
            GetSpawnAmount(out min, out max, bufferToSpawn);
            var amount = Random.Range(min, max);
            spawnData.BufferAmount = amount;
            spawnData.BufferKey = bufferToSpawn;
            spawnData.hasBuffer = true;
        }

        private void GetSpawnAmount(out int minAmount, out int maxAmount, PoolKeys poolKey)
        {
            for (int i = 0; i < enemyDatas.Length; i++)
            {
                if (enemyDatas[i].poolKey == poolKey)
                {
                    minAmount = enemyDatas[i].minSpawnAmount;
                    maxAmount = enemyDatas[i].maxSpawnAmount;
                    return;
                }
            }

            minAmount = 0;
            maxAmount = 0;
        }

        [Button]
        public void AddRandomEnemies()
        {
            SetPoolKeys();
            CheckBuffers();

            if (SpawnDatas == null)
                SpawnDatas = new List<SpawnData>();
            SpawnDatas.Clear();
            totalMonster = 0;
            totalSpawnTime = 0;
            round = 0;

            do
            {
                var enemyToAdd = Random.Range(minEnemySpawn, maxEnemySpawn);
                var decreaseTimeBetweenSpawns = round * spawnSpeedPerRound;
                if (decreaseTimeBetweenSpawns >= initialTimeBetweenEnemies)
                    decreaseTimeBetweenSpawns = initialTimeBetweenEnemies - 0.5f;
                var timeBetween = initialTimeBetweenEnemies - decreaseTimeBetweenSpawns;

                totalMonster += enemyToAdd;
                if (totalSpawnTime + timeBetween > floorTime)
                    timeBetween = floorTime - totalSpawnTime;

                totalSpawnTime += timeBetween;
                round++;

                SpawnData spawnData = new SpawnData();
                spawnData.Amount = enemyToAdd;
                spawnData.EnemyKey = poolKeys[Random.Range(0, poolKeys.Count)];
                spawnData.spawnWait = timeBetween;
                SpawnBuffer(spawnData);
                SpawnDatas.Add(spawnData);

                if (round > 100)
                    break;
            } while (totalSpawnTime < floorTime);
        }
    }

    [Serializable]
    public class SpawnData
    {
        public PoolKeys EnemyKey;
        public PoolKeys BufferKey;
        public bool hasBuffer;
        public int Amount;
        public int BufferAmount;
        public float spawnWait;
    }
}