using System;
using System.Collections.Generic;
using Data.LevelUp;
using Runtime.Enums;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.UIRelated.LevelUpRelated
{
    public class LevelUpUI : MonoBehaviour
    {
        public LevelUpStatsDropChancesSo levelUpStatsDropChancesSo;
        public List<LevelUpOptionUI> levelUpOptions;
        public List<LevelUpDropData> randomLevelUpDatas;

        public int levelUpAmount;

        private void Awake()
        {
            SetupLevelUpOptions();
            AddEvents();

            Debug.Log("Level Up Scene Opened!");
        }

        private void Start()
        {
            
        }

        private void SetupLevelUpOptions()
        {
            for (int i = 0; i < levelUpOptions.Count; i++)
            {
                levelUpOptions[i].SetLevelUpUI(this);
            }
        }

        [Button]
        public void RandomiseStats()
        {
            randomLevelUpDatas.Clear();
            for (int i = 0; i < levelUpOptions.Count; i++)
            {
                var stat = GetARandomStat();
                while (randomLevelUpDatas.Contains(stat))
                {
                    stat = GetARandomStat();
                }

                randomLevelUpDatas.Add(stat);
                levelUpOptions[i].SetStat(stat);
            }
        }

        private LevelUpDropData GetARandomStat()
        {
            var randWeight = Random.Range(0, 100f); //3
            var totalWeight = 0f;

            for (var i = 0; i < levelUpStatsDropChancesSo.levelUpDrops.Count; i++)
            {
                if (totalWeight + levelUpStatsDropChancesSo.levelUpDrops[i].percentage >= randWeight)
                    return levelUpStatsDropChancesSo.levelUpDrops[i];

                totalWeight += levelUpStatsDropChancesSo.levelUpDrops[i].percentage;
            }

            return null;
        }

        private void SetScreen()
        {
            if (levelUpAmount <= 0)
                OpenMarket();


            RandomiseStats();
        }

        private void OpenMarket()
        {
            EventManager.Instance.OpenScreen(Screens.Market, true);
        }

        private void OnSetLevelUpAmount(int levelNumber)
        {
            levelUpAmount = levelNumber;
            SetScreen();
        }

        public void LevelUpStatSelected()
        {
            levelUpAmount--;
            if (levelUpAmount <= 0)
                OpenMarket();

            RandomiseStats();
        }

        private void AddEvents()
        {
            EventManager.Instance.SetLevelUpAmountEvent += OnSetLevelUpAmount;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.SetLevelUpAmountEvent -= OnSetLevelUpAmount;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }

        public int timeToRoll;
        public int normalNumber;
        public int magicNumber;
        public int rareNumber;
        public int epicNumber;
        public int uniqueNumber;

        [Header("Chances")] public float normalPerc;
        public float magicPerc;
        public float rarePerc;
        public float epicPerc;
        public float uniquePerc;

        [Button]
        private void CalculateChances()
        {
            var normal = 0;
            var magic = 0;
            var rare = 0;
            var epic = 0;
            var unique = 0;
            for (int i = 0; i < timeToRoll; i++)
            {
                var stat = GetARandomStat();
                switch (stat.rarity)
                {
                    case ItemRarity.Normal:
                        normal++;
                        break;
                    case ItemRarity.Magic:
                        magic++;
                        break;
                    case ItemRarity.Rare:
                        rare++;
                        break;
                    case ItemRarity.Epic:
                        epic++;
                        break;
                    case ItemRarity.Unique:
                        unique++;
                        break;
                }
            }

            normalNumber = normal;
            magicNumber = magic;
            rareNumber = rare;
            epicNumber = epic;
            uniqueNumber = unique;

            normalPerc = 100f * ((float)normalNumber / (float)timeToRoll);
            magicPerc = 100f * ((float)magicNumber / (float)timeToRoll);
            rarePerc = 100f * ((float)rareNumber / (float)timeToRoll);
            epicPerc = 100f * ((float)epicNumber / (float)timeToRoll);
            uniquePerc = 100f * ((float)uniqueNumber / (float)timeToRoll);
        }
    }
}