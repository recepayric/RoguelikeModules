using System;
using System.Collections.Generic;
using Data.EnemyDataRelated;
using Runtime.Managers;
using TMPro;
using UnityEngine;

namespace Runtime.DebugRelated
{
    public class DebugEnemyListManager : MonoBehaviour
    {
        public int enemyIndex;
        public TextMeshProUGUI enemyStatText;
        public TextMeshProUGUI enemyNameText;
        
        public List<Enemy> addedEnemies;
        public List<DebugEnemyStats> enemyStats;

        private float towerBaseBuff;
        private float floorBaseBuff;

        public void NextEnemy()
        {
            enemyIndex++;
            if (enemyIndex >= addedEnemies.Count)
                enemyIndex = 0;
            
            UpdateEnemyStats();
        }

        public void PreviousEnemy()
        {
            enemyIndex--;

            if (enemyIndex < 0)
                enemyIndex = addedEnemies.Count-1;
            
            UpdateEnemyStats();
        }

        private void UpdateEnemyStats()
        {
            if (enemyStats.Count == 0)
            {
                enemyStatText.text = "";
                return;
            }
            var enemyData = enemyStats[enemyIndex];

            enemyNameText.text = enemyData.enemyName;
            var statText = "";

            statText += "<color=\"white\">Tower Stat Boost: <color=\"green\">" + towerBaseBuff + "\n";
            statText += "<color=\"white\">Floor Stat Boost: <color=\"green\">" + floorBaseBuff+ "\n";
            
            statText += "\n";
            statText += "<color=\"white\">Base Health: <color=\"green\">" + enemyData.baseHealth + "\n";
            statText += "<color=\"white\">Base Damage: <color=\"green\">" + enemyData.baseAttackDamage + "\n";
            statText += "<color=\"white\">Base Attack Speed: <color=\"green\">" + enemyData.baseaAttackSpeed + "\n";
            statText += "<color=\"white\">Base Attack Range: <color=\"green\">" + enemyData.baseAttackRange + "\n";
            statText += "<color=\"white\">Base Move Speed: <color=\"green\">" + enemyData.baseMoveSpeed + "\n";
            
            statText += "\n";
            statText += "<color=\"white\">Floor Health: <color=\"green\">" + enemyData.afterTowerBuffHealth + "\n";
            statText += "<color=\"white\">Floor Damage: <color=\"green\">" + enemyData.afterTowerBuffAttackDamage + "\n";
            statText += "<color=\"white\">Floor Attack Speed: <color=\"green\">" + enemyData.afterTowerBuffaAttackSpeed + "\n";
            statText += "<color=\"white\">Floor Attack Range: <color=\"green\">" + enemyData.afterTowerBuffAttackRange + "\n";
            statText += "<color=\"white\">Floor Move Speed: <color=\"green\">" + enemyData.afterTowerBuffMoveSpeed + "\n";
            
            statText += "\n";
            statText += "<color=\"white\">Modifier Health: <color=\"green\">" + enemyData.afterTowerStatBuffHealth + "\n";
            statText += "<color=\"white\">Modifier Damage: <color=\"green\">" + enemyData.afterTowerStatBuffAttackDamage + "\n";
            statText += "<color=\"white\">Modifier Attack Speed: <color=\"green\">" + enemyData.afterTowerStatBuffaAttackSpeed + "\n";
            statText += "<color=\"white\">Modifier Attack Range: <color=\"green\">" + enemyData.afterTowerStatBuffAttackRange + "\n";
            statText += "<color=\"white\">Modifier Move Speed: <color=\"green\">" + enemyData.afterTowerStatBuffMoveSpeed + "\n";
            
            enemyStatText.text = statText;
        }
        

        public void OnAddEnemyToTheList(Enemy enemy)
        {
            if (!CheckIfCanAddToTheList(enemy)) return;

            addedEnemies.Add(enemy);
            var data = new DebugEnemyStats();
            data.isUpdated = false;
            data.enemyName = enemy._stats.enemyData.enemyName;
            enemyStats.Add(data);

            if (enemyStats.Count == 1)
            {
                enemyIndex = 0;
                UpdateEnemyStats();
            }
        }

        private void OnUpdateEnemyBaseStats(Enemy enemy)
        {
            var result = CanUpdateStats(enemy);
            if (!result.Item1) return;
            
            var index = result.Item2;
            var data = enemyStats[index];

            data.baseHealth = enemy._stats.currentMaxHealth;
            data.baseAttackDamage = enemy._stats.currentDamage;
            data.baseAttackRange = enemy._stats.currentAttackRange;
            data.baseaAttackSpeed = enemy._stats.currentAttackSpeed;
            data.baseMoveSpeed = enemy._stats.currentSpeed;
        }

        private void OnUpdateEnemyTowerStats(Enemy enemy)
        {
            var result = CanUpdateStats(enemy);
            if (!result.Item1) return;
            
            var index = result.Item2;
            var data = enemyStats[index];

            data.afterTowerBuffHealth = enemy._stats.currentMaxHealth;
            data.afterTowerBuffAttackDamage = enemy._stats.currentDamage;
            data.afterTowerBuffAttackRange = enemy._stats.currentAttackRange;
            data.afterTowerBuffaAttackSpeed = enemy._stats.currentAttackSpeed;
            data.afterTowerBuffMoveSpeed = enemy._stats.currentSpeed;
        }

        private void OnUpdateEnemyModifierStats(Enemy enemy)
        {
            var result = CanUpdateStats(enemy);
            if (!result.Item1) return;
            
            var index = result.Item2;
            var data = enemyStats[index];
            
            data.afterTowerStatBuffHealth = enemy._stats.currentMaxHealth;
            data.afterTowerStatBuffAttackDamage = enemy._stats.currentDamage;
            data.afterTowerStatBuffAttackRange = enemy._stats.currentAttackRange;
            data.afterTowerStatBuffaAttackSpeed = enemy._stats.currentAttackSpeed;
            data.afterTowerStatBuffMoveSpeed = enemy._stats.currentSpeed;

            data.isUpdated = true;
            UpdateEnemyStats();
        }

        private void OnUpdateTowerStatBuffs(float tower, float floor)
        {
            towerBaseBuff = tower;
            floorBaseBuff = floor;
        }

        private (bool, int) CanUpdateStats(Enemy enemy)
        {
            for (int i = 0; i < enemyStats.Count; i++)
            {
                if (enemyStats[i].enemyName == enemy._stats.enemyData.enemyName && !enemyStats[i].isUpdated)
                    return (true, i);
            }

            return (false, -1);
        }

        private bool CheckIfCanAddToTheList(Enemy enemy)
        {
            string name = enemy._stats.enemyData.enemyName;

            for (int i = 0; i < addedEnemies.Count; i++)
            {
                if (addedEnemies[i]._stats.enemyData.enemyName == name)
                    return false;
            }

            return true;
        }

        private void ResetList()
        {
            enemyStats.Clear();
            addedEnemies.Clear();
            for (int i = 0; i < enemyStats.Count; i++)
            {
                enemyStats[i].isUpdated = false;
            }

            enemyIndex = 0;
        }

        private void Start()
        {
            DebugEvents.Instance.AddEnemyToDebugListEvent += OnAddEnemyToTheList;
            DebugEvents.Instance.UpdateEnemyBaseStatsEvent += OnUpdateEnemyBaseStats;
            DebugEvents.Instance.UpdateEnemyTowerStatsEvent += OnUpdateEnemyTowerStats;
            DebugEvents.Instance.UpdateEnemyModifierStatsEvent += OnUpdateEnemyModifierStats;
            DebugEvents.Instance.UpdateTowerStatBoostValuesEvent += OnUpdateTowerStatBuffs;

            EventManager.Instance.FloorStartsEvent += ResetList;
        }

        private void OnDestroy()
        {
            DebugEvents.Instance.AddEnemyToDebugListEvent -= OnAddEnemyToTheList;
            DebugEvents.Instance.UpdateEnemyBaseStatsEvent -= OnUpdateEnemyBaseStats;
            DebugEvents.Instance.UpdateEnemyTowerStatsEvent -= OnUpdateEnemyTowerStats;
            DebugEvents.Instance.UpdateEnemyModifierStatsEvent -= OnUpdateEnemyModifierStats;
            DebugEvents.Instance.UpdateTowerStatBoostValuesEvent -= OnUpdateTowerStatBuffs;
            EventManager.Instance.FloorStartsEvent -= ResetList;
        }
    }

    [Serializable]
    public class DebugEnemyStats
    {
        public string enemyName;
        public bool isUpdated;
        public float baseHealth;
        public float baseAttackDamage;
        public float baseaAttackSpeed;
        public float baseMoveSpeed;
        public float baseAttackRange;

        public float afterTowerBuffHealth;
        public float afterTowerBuffAttackDamage;
        public float afterTowerBuffaAttackSpeed;
        public float afterTowerBuffMoveSpeed;
        public float afterTowerBuffAttackRange;

        public float afterTowerStatBuffHealth;
        public float afterTowerStatBuffAttackDamage;
        public float afterTowerStatBuffaAttackSpeed;
        public float afterTowerStatBuffMoveSpeed;
        public float afterTowerStatBuffAttackRange;
    }
}