using System;
using Data;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.UIRelated;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class PlayerLevel : MonoBehaviour
    {
        private CharacterDataSo _characterDataSo;
        private Stats _playerStats;

        public int startLevel;
        public int levelDifference;
        public int level;
        public float baseNeededExperience;
        public float neededExperience;
        public float experienceCollected;
        public float experienceCollectedTotal;
        public float experienceMultiplier;
        public float experienceNeededIncrease;

        private void Start()
        {
            AddEvents();
        }

        public void SetPlayerData(CharacterDataSo characterDataSo, Stats stats)
        {
            _characterDataSo = characterDataSo;
            _playerStats = stats;
            SetBaseStats();
        }

        private void SetBaseStats()
        {
            experienceMultiplier = _characterDataSo.baseExpGainMultiplier;
            experienceCollected = 0;
            baseNeededExperience = _characterDataSo.baseExpNeeded;
            experienceNeededIncrease = _characterDataSo.baseExpNeedIncrease;

            CalculateNeededExperience();
            IncreaseExperience(0);
        }


        public void OnOrbCollected(float value)
        {
            var newValue = (value + value * _playerStats.GetStat(AllStats.ExpGainMultiplier) / 100f) *
                           experienceMultiplier;
            IncreaseExperience(newValue);
        }

        private void IncreaseExperience(float value)
        {
            experienceCollected += value;
            experienceCollectedTotal += value;

            CheckIfLevelUp();

            var perc = experienceCollected / neededExperience;
            EventManager.Instance.UpdateLevelProgress(perc, experienceCollected, neededExperience);
        }

        private void LevelUp()
        {
            level++;
            experienceCollected -= neededExperience;
            CalculateNeededExperience();

            //todo particles and etc!
        }

        private void CheckIfLevelUp()
        {
            while (experienceCollected >= neededExperience)
            {
                LevelUp();
            }
        }

        private void CalculateNeededExperience()
        {
            neededExperience = baseNeededExperience * Mathf.Pow(experienceNeededIncrease, level - 1);
        }

        private void OnFloorStarts()
        {
            startLevel = level;
        }

        private void OnFloorEnds(int floorNum)
        {
            levelDifference = level - startLevel;
            Debug.Log("Opening level up scene");
            EventManager.Instance.OpenScreen(Screens.LevelUp, true);
            Debug.Log("Setting level up amount!");
            EventManager.Instance.SetLevelUpAmount(levelDifference);
        }

        private void AddEvents()
        {
            EventManager.Instance.OrbCollectedEvent += OnOrbCollected;
            EventManager.Instance.FloorStartsEvent += OnFloorStarts;
            EventManager.Instance.FloorEndsEvent += OnFloorEnds;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.OrbCollectedEvent -= OnOrbCollected;
            EventManager.Instance.FloorStartsEvent -= OnFloorStarts;
            EventManager.Instance.FloorEndsEvent -= OnFloorEnds;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }
    }
}