using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
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

        public void Initialise()
        {
            EventManager.Instance.FloorStartsEvent += StartSpawning;
        }

        public void Setup()
        {
            totalWaveTime = enemySpawnDataSo.floorTime;
            GameConfig.FloorDuration = (int)totalWaveTime;
            SpawnDatas = enemySpawnDataSo.SpawnDatas;

            isStarted = false;
            waveSpawned = 0;
            totalWave = SpawnDatas.Count;
            spawnTime = totalWaveTime / totalWave;
            //totalWave = (int)(totalWaveTime / spawnTime);
        }

        public void SpawnMonster()
        {
            for (int i = 0; i < SpawnDatas[waveSpawned].Amount; i++)
            {
                var randX = Random.Range(-xBound, xBound);
                var randY = Random.Range(-yBound, yBound);
                //Debug.Log("Type: " + SpawnDatas[waveSpawned].EnemyKey);
                var enemy = BasicPool.instance.Get(SpawnDatas[waveSpawned].EnemyKey);
                enemy.transform.position = new Vector2(randX, randY);
            }

            //spawn buffer here!!
            for (int i = 0; i < SpawnDatas[waveSpawned].BufferAmount; i++)
            {
                var randX = Random.Range(-xBound, xBound);
                var randY = Random.Range(-yBound, yBound);

                var enemy = BasicPool.instance.Get(SpawnDatas[waveSpawned].BufferKey);
                enemy.transform.position = new Vector2(randX, randY);
            }

            waveSpawned++;

            var waitTime = 3f;

            if(waveSpawned < SpawnDatas.Count)
                waitTime = SpawnDatas[waveSpawned].spawnWait;
            
            Debug.Log("Spawning a group of monsters!!! " + waitTime);


            DOVirtual.DelayedCall(waitTime, () =>
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
            EventManager.Instance.SetMonsterSpawn(false);
        }

        public void StartSpawning()
        {
            if (isStarted)
            {
                if (!DOTween.IsTweening(SpawnID))
                    DOTween.Play(SpawnID);
            }
            else
            {
                Setup();
                SpawnMonster();
            }

            EventManager.Instance.SetMonsterSpawn(true);
        }


        public void Destroy()
        {
            EventManager.Instance.FloorStartsEvent -= StartSpawning;
        }
    }
}