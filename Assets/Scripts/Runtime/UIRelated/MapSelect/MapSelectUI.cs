using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Runtime.UIRelated.MapSelect
{
    public class MapSelectUI : MonoBehaviour, IOpenable
    {
        public MapDetailUI mapDetailUI;
        private const string PickerMoveID = "PickerMove";
        public int currentTier;
        public int selectedTier;
        public Image towerImage;
        public List<Sprite> tutorialTowerImages;
        public List<Sprite> tieredTowerImages;
        public List<Sprite> bossTowerImages;
        public List<RectTransform> tutorialMaps;
        public List<RectTransform> bossMaps;
        public RectTransform tieredMap;

        public int selectedTutorialMap;
        public int selectedBossMap;
        public RectTransform pickerImage;
        public TextMeshProUGUI mapTierText;

        public PickTypes pickedMapType;

        private void Start()
        {
            currentTier = 0;
            selectedTier = 0;
            //EventManager.Instance.PrepareTower(currentTier+1);
            mapDetailUI.SetTier(currentTier + 1);
            mapDetailUI.UpdateTowerDetails();
        }

        private void ChangeTowerImage()
        {
            var image = tieredTowerImages[0];
            
            if (pickedMapType == PickTypes.Tiered)
                image = tieredTowerImages[currentTier];
            else if (pickedMapType == PickTypes.Boss)
                image = bossTowerImages[selectedBossMap];
            else if (pickedMapType == PickTypes.Tutorial)
                image = tutorialTowerImages[selectedTutorialMap];

            var imageX = image.bounds.size.x;
            var imageY = image.bounds.size.y;
            var ratio = imageY / imageX;
            
            towerImage.sprite = image;

            var currentX = towerImage.rectTransform.sizeDelta.x;
            var targetY = currentX * ratio;
            towerImage.rectTransform.sizeDelta = new Vector2(currentX, targetY);
        }
        
        public void SelectTutorialMap(int tier)
        {
            pickedMapType = PickTypes.Tutorial;
            selectedTutorialMap = tier;
            selectedTier = selectedTutorialMap + MapConfig.TutorialMapOffset;

            CheckPickerMove();
            pickerImage.DOAnchorPosX(tutorialMaps[tier].anchoredPosition.x, UIConfig.PickerMoveTime)
                .SetId(PickerMoveID);
            
            ChangeTowerImage();
            ChangeMapTier();
        }

        public void SelectTieredMaps()
        {
            pickedMapType = PickTypes.Tiered;
            selectedTier = currentTier;
            CheckPickerMove();
            pickerImage.DOAnchorPosX(tieredMap.anchoredPosition.x, UIConfig.PickerMoveTime).SetId(PickerMoveID);
            ChangeTowerImage();
            ChangeMapTier();
        }

        public void SelectBossMap(int tier)
        {
            pickedMapType = PickTypes.Boss;
            selectedBossMap = tier;

            CheckPickerMove();
            pickerImage.DOAnchorPosX(bossMaps[tier].anchoredPosition.x, UIConfig.PickerMoveTime).SetId(PickerMoveID);
            ChangeTowerImage();
            ChangeMapTier();

        }

        public void IncreaseTier()
        {
            if (pickedMapType != PickTypes.Tiered) return;

            if (currentTier < 15)
                currentTier++;

            selectedTier = currentTier;
            UpdateTierText();
            ChangeMapTier();
        }

        public void DecreaseTier()
        {
            if (currentTier > 0)
                currentTier--;

            selectedTier = currentTier;
            UpdateTierText();
            ChangeMapTier();
        }

        private void UpdateTierText()
        {
            mapTierText.text = (currentTier + 1).ToString();
        }

        private void ChangeMapTier()
        {
            ChangeTowerImage();
            mapDetailUI.SetTier(selectedTier);
            //mapDetailUI.UpdateTowerDetails();
        }
        

        private void CheckPickerMove()
        {
            if (DOTween.IsTweening(PickerMoveID))
                DOTween.Kill(PickerMoveID);
        }

        public void StartTower()
        {
            EventManager.Instance.LoadTower();
        }

        public void ReturnToCharacterSelectScreen()
        {
            EventManager.Instance.OpenScreen(Screens.CharacterSelect, true);
        }

        public void OnOpened()
        {
            UpdateTierText();
        }

        public void OnClosed()
        {
        }
    }

    public enum PickTypes
    {
        Tutorial,
        Tiered,
        Boss
    }
}