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
        public int floorNumber;
        public float floorTime;
        public Object[] enemyDataObjects;
        //public EnemyData[] enemyDatas;
        public List<EnemyData> enemyDatas;
        public List<List<SpawnData>> SpawnDatas;
        public List<float> floorTimes;

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
        public float enemyNumberIncreaesPerFloor;

        [Button]
        public void GetAllResources()
        {
            enemyDataObjects = Resources.LoadAll("EnemyDatas", typeof(EnemyData));
            enemyDatas = new List<EnemyData>();
            for (int i = 0; i < enemyDataObjects.Length; i++)
            {
                var data = (EnemyData)enemyDataObjects[i];
                if (data.CanBeSpawnedNormally)
                    enemyDatas.Add(data);
            }
        }


        private void SetPoolKeys()
        {
            poolKeys.Clear();
            for (int i = 0; i < enemyDatas.Count; i++)
            {
                if(enemyDatas[i] == null)
                    continue;
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
            for (int i = 0; i < enemyDatas.Count; i++)
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

        private void FillSpawnData()
        {
            SetPoolKeys();
            CheckBuffers();
            if (SpawnDatas.Count <= floor)
                SpawnDatas.Add(new List<SpawnData>());
            else
                SpawnDatas[floor].Clear();
            
            totalSpawnTime = 0;
            round = 0;
            var enemyNumIncrease = floor * enemyNumberIncreaesPerFloor;

            do
            {
                var enemyToAdd = Random.Range(minEnemySpawn, maxEnemySpawn);
                enemyToAdd += (int)(enemyToAdd * enemyNumIncrease);
                var decreaseTimeBetweenSpawns = round * spawnSpeedPerRound;
                if (decreaseTimeBetweenSpawns >= initialTimeBetweenEnemies)
                    decreaseTimeBetweenSpawns = initialTimeBetweenEnemies - 1f;
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
                SpawnDatas[floor].Add(spawnData);

                if (round > 100)
                    break;
            } while (totalSpawnTime < floorTime);
        }

        [Button]
        public void AddRandomEnemies()
        {
            if (SpawnDatas == null)
                SpawnDatas = new List<List<SpawnData>>();
            
            totalMonster = 0;
            floor = 0;
            floorTimes.Clear();
            
            
            for (int i = 0; i < floorNumber; i++)
            {
                floorTime = 30 + floor * 5;
                if (floorTime > 60)
                    floorTime = 60;
                
                floorTimes.Add(floorTime);
                FillSpawnData();
                floor++;
            }
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