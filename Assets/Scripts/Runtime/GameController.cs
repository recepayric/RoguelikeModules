using System;
using System.Collections;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.WorldRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    #region instance

    public static GameController instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion
    
    public EnemySpawner enemySpawner;
    public FloorInitializer floorInitializer;
    
    public float xBound = 8.5f;
    public float yBound = 4.5f;

    public bool spawnMonsters = true;
    public bool spawnOneTime = false;
    public int spawnAmount = 10;
    public float spawnTime = 3;

    [SerializeField] private CurrencyDataSo _currencyDataSo;

    
    void Start()
    {
        floorInitializer.Initialise();
        enemySpawner.Initialise();
        
        SetSpawnEnemyTimer();
        _currencyDataSo = Resources.Load<CurrencyDataSo>("CollectableData");
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartSpawningMonsters();
        if (Input.GetKeyDown(KeyCode.O))
            StopSpawningMonsters();
    }


    public void OrbCollected()
    {
        
    }
    
    

    [Button]
    public void StartSpawningMonsters()
    {
        enemySpawner.StartSpawning();
    }

    [Button]
    public void StopSpawningMonsters()
    {
        enemySpawner.StopSpawning();
    }

   

    private void SetSpawnEnemyTimer()
    {
        if (spawnMonsters)
        {
            SpawnEnemies();
            if (spawnOneTime)
                spawnMonsters = false;
        }

        if (spawnTime == 0)
            spawnTime = 0.1f;
        DOVirtual.DelayedCall(spawnTime, SetSpawnEnemyTimer);
    }

    [Button]
    public void SpawnEnemies()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            var randX = Random.Range(-xBound, xBound);
            var randY = Random.Range(-yBound, yBound);

            var enemy = BasicPool.instance.Get(PoolKeys.Enemy1);
            enemy.transform.position = new Vector3(randX, randY, 0);
        }
    }


    private void OnDestroy()
    {
        floorInitializer.Destroy();
    }
}