using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.WorldRelated
{
    [Serializable]
    public class EnemySpawner
    {
        private const string SpawnID = "monster_spawner";
        public EnemySpawnDataSo enemySpawnDataSo;
        public float xBound = 17;
        public float yBound = 9f;


        public int floorNumber;

        [Header("Spawn Time/Amount")] public float totalWaveTime = 60f;
        public float spawnTime;
        public float totalWave;
        public int waveSpawned;

        public List<SpawnData> SpawnDatas;

        private bool isStarted = false;

        public void Setup()
        {
            isStarted = false;
            waveSpawned = 0;
            totalWave = 0;
            spawnTime = 5f;
            totalWave = (int)(totalWaveTime / spawnTime);
            
            SpawnDatas = enemySpawnDataSo.SpawnDatas;
        }

        public void SpawnMonster()
        {
            Debug.Log("Spawning Enemy!");
            for (int i = 0; i < SpawnDatas[waveSpawned].Amount; i++)
            {
                var randX = Random.Range(-xBound, xBound);
                var randY = Random.Range(-yBound, yBound);

                var enemy = BasicPool.instance.Get(SpawnDatas[waveSpawned].EnemyKey);
                enemy.transform.position = new Vector2(randX, randY);
            }

            waveSpawned++;

            DOVirtual.DelayedCall(spawnTime, () =>
            {
                if (waveSpawned < totalWave)
                    SpawnMonster();
                else
                    isStarted = false;
            }).SetId(SpawnID);

            isStarted = true;
        }


        public void StopSpawning()
        {
            DOTween.Pause(SpawnID);
        }

        public void StartSpawning()
        {
            if(isStarted)
                DOTween.Play(SpawnID);
            else
            {
                Setup();
                SpawnMonster();
            }
        }
    }
}