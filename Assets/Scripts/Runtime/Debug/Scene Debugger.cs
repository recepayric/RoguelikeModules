using System.Collections;
using System.Collections.Generic;
using Runtime.Managers;
using Runtime.TowerRelated;
using Runtime.WorldRelated;
using Sirenix.OdinInspector;
using UnityEngine;

public class SceneDebugger : MonoBehaviour
{
    public int towerTier;
    public int floorNum;
    public Tower preparedTower;
    public EnemySpawner Spawner;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [Button]
    public void SpawnMonsters()
    {
        Spawner.StartSpawning(floorNum);
    }

    [Button]
    public void PrepareTower()
    {
        EventManager.Instance.PrepareTower(towerTier);
    }
    
    public void OnUpdateTower(Tower tower)
    {
        preparedTower = tower;
    }
    
    private void Awake()
    {
        EventManager.Instance.UpdateTowerEvent += OnUpdateTower;
    }

    private void OnDestroy()
    {
        EventManager.Instance.UpdateTowerEvent -= OnUpdateTower;
    }
}
