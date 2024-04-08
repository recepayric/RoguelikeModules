using Runtime.Configs;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.Managers
{
    public class DebugEvents : MonoBehaviour
    {
        public static DebugEvents Instance;

        private void Awake()
        {
            Instance = this;
        }
        
        
        //Debug Events
        public event UnityAction<Enemy> AddEnemyToDebugListEvent;

        public void AddEnemyToDebugList(Enemy enemy)
        {
            if (GameConfig.IsDebug) AddEnemyToDebugListEvent?.Invoke(enemy);
        }
        
        public event UnityAction<Enemy> UpdateEnemyBaseStatsEvent;

        public void UpdateEnemyBaseStats(Enemy enemy)
        {
            if (GameConfig.IsDebug) UpdateEnemyBaseStatsEvent?.Invoke(enemy);
        }
        
        public event UnityAction<Enemy> UpdateEnemyTowerStatsEvent;

        public void UpdateEnemyTowerStats(Enemy enemy)
        {
            if (GameConfig.IsDebug) UpdateEnemyTowerStatsEvent?.Invoke(enemy);
        }
        
        public event UnityAction<Enemy> UpdateEnemyModifierStatsEvent;

        public void UpdateEnemyModifierStats(Enemy enemy)
        {
            if (GameConfig.IsDebug) UpdateEnemyModifierStatsEvent?.Invoke(enemy);
        }
        
        public event UnityAction<float, float> UpdateTowerStatBoostValuesEvent;

        public void UpdateTowerStatBoostValues(float tower, float floor)
        {
            if (GameConfig.IsDebug) UpdateTowerStatBoostValuesEvent?.Invoke(tower, floor);
        }
    }
}