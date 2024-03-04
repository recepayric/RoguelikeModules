using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.TowerRelated;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.WorldRelated
{
    [Serializable]
    public class EnemySpawner
    {
        private const string SpawnID = "monster_spawner";
        public Tower selectedTower;
        public EnemySpawnDataSo tutorialTower1DataSo;
        public EnemySpawnDataSo tutorialTower2DataSo;
        public EnemySpawnDataSo tutorialTower3DataSo;
        public EnemySpawnDataSo enemySpawnDataSo;
        public float xBound = 17;
        public float yBound = 9f;

        public int floorNumber;

        [Header("Spawn Time/Amount")] public float totalWaveTime = 60f;
        public float spawnTime;
        public float totalWave;
        public int waveSpawned;

        public List<SpawnData> SpawnDatas;

        public bool isStarted = false;

        public void Initialise()
        {
            //EventManager.Instance.FloorStartsEvent += StartSpawning;
            EventManager.Instance.FloorEndsEvent += OnFloorEnds;
            EventManager.Instance.UpdateTowerEvent += OnUpdateTower;
        }

        private EnemySpawnDataSo UpdateSpawnData()
        {
            var data = enemySpawnDataSo;
            if (selectedTower.tier == -3)
                data = tutorialTower1DataSo;
            else if (selectedTower.tier == -3)
                data = tutorialTower1DataSo;
            else if (selectedTower.tier == -3)
                data = tutorialTower1DataSo;

            return data;
        }
        
        public void Setup()
        {
            var data = UpdateSpawnData();
            Debug.Log(floorNumber);
            totalWaveTime = data.floorTimes[floorNumber];
            SpawnDatas = data.SpawnDatas[floorNumber];

           
            GameConfig.FloorDuration = (int)totalWaveTime;
            isStarted = false;
            waveSpawned = 0;
            totalWave = SpawnDatas.Count;
            spawnTime = totalWaveTime / totalWave;
            Debug.Log("Spawner Set Up!!");
            //totalWave = (int)(totalWaveTime / spawnTime);
        }

        public void SpawnMonster()
        {
            
            if (SpawnDatas[waveSpawned].hasBoss)
            {
                var randX = Random.Range(-xBound, xBound);
                var randY = Random.Range(-yBound, yBound);

                var enemy = BasicPool.instance.Get(SpawnDatas[waveSpawned].BossKey);
                enemy.transform.position = new Vector2(randX, randY);
                enemy.name = "Boss " + Random.Range(0, 100000);
                return;
            }
            
            for (int i = 0; i < SpawnDatas[waveSpawned].Amount; i++)
            {
                var randX = Random.Range(-xBound, xBound);
                var randY = Random.Range(-yBound, yBound);
                //Debug.Log("Type: " + SpawnDatas[waveSpawned].EnemyKey);
                var enemy = BasicPool.instance.Get(SpawnDatas[waveSpawned].EnemyKey);
                enemy.transform.position = new Vector2(randX, randY);
                enemy.name = "Enemy " + Random.Range(0, 100000);
            }

            //spawn buffer here!!
            for (int i = 0; i < SpawnDatas[waveSpawned].BufferAmount; i++)
            {
                var randX = Random.Range(-xBound, xBound);
                var randY = Random.Range(-yBound, yBound);

                var enemy = BasicPool.instance.Get(SpawnDatas[waveSpawned].BufferKey);
                enemy.transform.position = new Vector2(randX, randY);
                enemy.name = "Buffer " + Random.Range(0, 100000);
            }
            
            //Spawn Boss Here
           

            waveSpawned++;

            var waitTime = 3f;

            if(waveSpawned < SpawnDatas.Count)
                waitTime = SpawnDatas[waveSpawned].spawnWait;
            
            DOVirtual.DelayedCall(waitTime, () =>
            {
                if (waveSpawned < totalWave)
                    SpawnMonster();
                else
                    isStarted = false;
            }).SetId(SpawnID);
        }


        public void StopSpawning()
        {
            DOTween.Pause(SpawnID);
            EventManager.Instance.SetMonsterSpawn(false);
        }

        public void StartSpawning(int floorNum)
        {
            floorNumber = floorNum;
            if (isStarted)
            {
                if (!DOTween.IsTweening(SpawnID))
                    DOTween.Play(SpawnID);
            }
            else
            {
                Setup();
                isStarted = true;
                SpawnMonster();
            }

            EventManager.Instance.SetMonsterSpawn(true);
        }

        private void OnFloorEnds(int num)
        {
            if (DOTween.IsTweening(SpawnID))
                DOTween.Kill(SpawnID);

            isStarted = false;
        }


        private void OnUpdateTower(Tower tower)
        {
            selectedTower = tower;
        }

        public void Destroy()
        {
            //EventManager.Instance.FloorStartsEvent -= StartSpawning;
            EventManager.Instance.FloorEndsEvent -= OnFloorEnds;
            EventManager.Instance.UpdateTowerEvent -= OnUpdateTower;
        }
    }
}