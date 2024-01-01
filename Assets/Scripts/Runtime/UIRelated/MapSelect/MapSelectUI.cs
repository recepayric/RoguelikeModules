using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.UIRelated.MapSelect
{
    public class MapSelectUI : MonoBehaviour, IOpenable
    {
        public MapDetailUI mapDetailUI;
        private const string MapMoveID = "MapMove";
        private const string MapScaleUpID = "MapScaleUp";
        private const string MapScaleDownID = "MapScaleDown";
        public int currentTier;
        public int previousSelectedTier;
        public List<RectTransform> mapImages;

        private void Start()
        {
            currentTier = 0;
            ArrangeMaps();
            
            //EventManager.Instance.PrepareTower(currentTier+1);
            mapDetailUI.SetTier(currentTier+1);
            mapDetailUI.UpdateTowerDetails();
        }

        public float widthOfImage = 200f;
        public float gap = 30f;
        public float moveTime;
        public float scaleTime;

        [Button]
        public void ArrangeMaps()
        {
            for (int i = 0; i < mapImages.Count; i++)
            {
                mapImages[i].anchoredPosition = new Vector2(i * (widthOfImage + gap), 0);
            }

            ChangeMapOrder();
        }

        public void ChangeMapOrder()
        {
            DOTween.Kill(MapMoveID);
            //DOTween.Complete(MapScaleID);

            for (int i = 0; i < mapImages.Count; i++)
            {
                var index = i - currentTier;
                var targetPos = new Vector2(index * (widthOfImage + gap), 0);
                mapImages[i].DOAnchorPos(targetPos, moveTime).SetId(MapMoveID);
            }

            DOTween.Complete(MapScaleDownID);
            DOTween.Kill(MapScaleUpID);

            DOVirtual.DelayedCall(moveTime, () =>
            {
                mapImages[currentTier].transform.DOScale(Vector3.one * UIConfig.SelectedMapScaleUpValue, scaleTime)
                    .SetId(MapScaleUpID);
                mapImages[previousSelectedTier].transform.DOScale(Vector3.one, scaleTime).SetId(MapScaleDownID);

                ChangeMapTier();
            });
        }

        public void SelectMap(int index)
        {
            if (index == currentTier) return;
            previousSelectedTier = currentTier;
            currentTier = index;
            ChangeMapOrder();
        }

        private void ChangeMapTier()
        {
            mapDetailUI.SetTier(currentTier+1);
            mapDetailUI.UpdateTowerDetails();
        }
        
        public void StartTower()
        {
            EventManager.Instance.LoadTower();
        }

        public void OnOpened()
        {
            ArrangeMaps();
        }

        public void OnClosed()
        {
        }
    }
}