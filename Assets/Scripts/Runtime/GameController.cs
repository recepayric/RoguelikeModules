using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.WeaponDataRelated;
using DG.Tweening;
using Runtime;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.UIRelated;
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
    private Player player;

    public float xBound = 8.5f;
    public float yBound = 4.5f;

    public bool spawnMonsters = true;
    public bool spawnOneTime = false;
    public int spawnAmount = 10;
    public float spawnTime = 3;

    [SerializeField] private CurrencyDataSo _currencyDataSo;

    public CharacterDataSo selectedCharacter;
    public WeaponDataSo starterWeapon;

    public bool isPlayerCreated = false;
    public bool isNewPlayer = false;
    public bool isPlayerDead = false;


    void Start()
    {
        floorInitializer.Initialise();
        enemySpawner.Initialise();

        //SetSpawnEnemyTimer();
        _currencyDataSo = Resources.Load<CurrencyDataSo>("CollectableData");

        AddEvents();
        GameStart();
    }

    private void GameStart()
    {
        EventManager.Instance.GameStart();
        EventManager.Instance.OpenScreen(GameConfig.FirstScreenToOpen, true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            StartSpawningMonsters();
        if (Input.GetKeyDown(KeyCode.O))
            StopSpawningMonsters();
    }

    [Button]
    public void StartSpawningMonsters()
    {
        enemySpawner.StartSpawning(floorInitializer.currentFloor);
    }

    [Button]
    public void StopSpawningMonsters()
    {
        enemySpawner.StopSpawning();
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

    private void OnLoadLevel()
    {
        //Load Player here!!!
        Debug.Log("Player is being created");
        CreatePlayer();
        Debug.Log("Starter weapon is being given!");
        EventManager.Instance.OpenScreen(Screens.Floor, true);
        EventManager.Instance.FloorLoad();
    }

    private void OnFlooLoad()
    {
        StartSpawningMonsters();
        floorInitializer.OnFloorLoad();
    }

    private void OnFloorEnd(int floorNumber)
    {
        var playerLevelDiff = player.playerLevel.CalculateLevelUp();
        KillAllEnemies();

        //todo check if player is in endless mode!!!
        if (floorInitializer.isPlayerDead)
        {
            EventManager.Instance.OpenScreen(Screens.GameEnd, true);
        }
        else if (floorNumber >= 20)
        {
            EventManager.Instance.GameEnd(true);
        }
        else if (playerLevelDiff > 0)
        {
            EventManager.Instance.OpenScreen(Screens.LevelUp, true);
            EventManager.Instance.SetLevelUpAmount(playerLevelDiff);
        }
        else
        {
            EventManager.Instance.OpenScreen(Screens.Market, true);
        }
    }

    private void CreatePlayer()
    {
        if (isPlayerCreated && isNewPlayer)
        {
            BasicPool.instance.Return(this.player.gameObject);
        }else if (isPlayerCreated) return;
        
        var player = BasicPool.instance.Get(selectedCharacter.playerPoolKey);
        player.transform.position = Vector3.zero;
        isPlayerCreated = true;
        this.player = DictionaryHolder.Player;
        this.player.SetStarterWeapon(starterWeapon.WeaponPoolKey);
        isNewPlayer = false;
    }

    private void OnCharacterSelected(CharacterDataSo characterDataSo)
    {
        selectedCharacter = characterDataSo;
        isNewPlayer = true;
    }

    private void OnWeaponSelected(WeaponDataSo weaponDataSo)
    {
        starterWeapon = weaponDataSo;
    }


    private void OnGameEnds(int floorNum)
    {
    }

    private void KillAllEnemies()
    {
        var enemies = DictionaryHolder.Enemies.Keys.ToList();
        for (int i = 0; i < enemies.Count; i++)
        {
            BasicPool.instance.Return(enemies[i]);
        }
    }


    private void AddEvents()
    {
        EventManager.Instance.LoadTowerEvent += OnLoadLevel;
        EventManager.Instance.CharacterSelectedEvent += OnCharacterSelected;
        EventManager.Instance.WeaponSelectedEvent += OnWeaponSelected;
        EventManager.Instance.FloorEndsEvent += OnGameEnds;
        EventManager.Instance.FloorLoadEvent += OnFlooLoad;
        EventManager.Instance.FloorEndsEvent += OnFloorEnd;
    }

    private void RemoveEvents()
    {
        EventManager.Instance.LoadTowerEvent -= OnLoadLevel;
        EventManager.Instance.CharacterSelectedEvent -= OnCharacterSelected;
        EventManager.Instance.WeaponSelectedEvent -= OnWeaponSelected;
        EventManager.Instance.FloorEndsEvent -= OnGameEnds;
        EventManager.Instance.FloorLoadEvent -= OnFlooLoad;
        EventManager.Instance.FloorEndsEvent -= OnFloorEnd;
    }

    private void OnDestroy()
    {
        RemoveEvents();
        floorInitializer.Destroy();
    }
}