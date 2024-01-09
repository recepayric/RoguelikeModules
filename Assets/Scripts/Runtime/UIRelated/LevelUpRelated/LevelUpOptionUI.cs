using System;
using System.Collections.Generic;
using Data.LevelUp;
using DG.Tweening;
using Runtime.Configs;
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
        public TextMeshProUGUI statName;

        public AllStats stat;
        public float amount;
        public LevelUpStats levelUpStats;

        public Image image;
        public List<Sprite> cardsForTiers;
        public Sprite closedImage;

        public void SetLevelUpUI(LevelUpUI pLevelUpUI)
        {
            levelUpUI = pLevelUpUI;
        }

        public void SetStat(LevelUpDropData data)
        {
            stat = data.stat;
            amount = data.value;
            
            headerText.text = stat.ToString();
            statText.text = "+" + amount;
            statName.text = stat.ToString();
            TurnAround(data.rarity);
        }

        private void TurnAround(ItemRarity rarity)
        {
            image.sprite = closedImage;
            transform.DOScaleX(0, UIConfig.LevelUpCardTurnTime/2f).OnComplete(() =>
            {
                SwitchImage(rarity);
                transform.DOScaleX(1, UIConfig.LevelUpCardTurnTime / 2f);
            });
        }

        private void SwitchImage(ItemRarity rarity)
        {
            switch (rarity)
            {
                case ItemRarity.Normal:
                    image.sprite = cardsForTiers[0];
                    break;
                case ItemRarity.Magic:
                    image.sprite = cardsForTiers[1];
                    break;
                case ItemRarity.Rare:
                    image.sprite = cardsForTiers[2];
                    break;
                case ItemRarity.Epic:
                    image.sprite = cardsForTiers[3];
                    break;
                case ItemRarity.Unique:
                    image.sprite = cardsForTiers[4];
                    break;
            }
        }
        
        public void SelectItem()
        {
            levelUpStats = new LevelUpStats(stat, amount);
            EventManager.Instance.LevelUpStatSelected(levelUpStats);
            levelUpUI.LevelUpStatSelected();
        }
    }
}