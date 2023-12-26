using System;
using Data.LevelUp;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.StatValue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UIRelated.LevelUpRelated
{
    public class LevelUpOptionUI : MonoBehaviour
    {
        public LevelUpUI levelUpUI;
        public TextMeshProUGUI headerText;
        public TextMeshProUGUI statText;

        public AllStats stat;
        public float amount;
        public LevelUpStats levelUpStats;

        public Image image;
        public Color colorNormal;
        public Color colorMagic;
        public Color colorRare;
        public Color colorEpic;
        public Color colorUnique;

        public void SetLevelUpUI(LevelUpUI pLevelUpUI)
        {
            levelUpUI = pLevelUpUI;
        }

        public void SetStat(LevelUpDropData data)
        {
            stat = data.stat;
            amount = data.value;

            switch (data.rarity)
            {
                case ItemRarity.Normal:
                    image.color = colorNormal;
                    break;
                case ItemRarity.Magic:
                    image.color = colorMagic;
                    break;
                case ItemRarity.Rare:
                    image.color = colorRare;
                    break;
                case ItemRarity.Epic:
                    image.color = colorEpic;
                    break;
                case ItemRarity.Unique:
                    image.color = colorUnique;
                    break;
            }

            headerText.text = stat.ToString();
            statText.text = "+" + amount + " " + stat.ToString();
        }
        
        public void SelectItem()
        {
            levelUpStats = new LevelUpStats(stat, amount);
            EventManager.Instance.LevelUpStatSelected(levelUpStats);
            levelUpUI.LevelUpStatSelected();
        }
    }
}