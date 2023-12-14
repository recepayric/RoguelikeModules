using System;
using System.Collections.Generic;
using Data;
using DG.Tweening;
using Runtime.Enums;
using Runtime.UIRelated.WeaponUpgrades;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace Runtime.UIRelated.CharacterSelect
{
    public class CharacterFrameUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler, IPoolObject
    {
        private CharacterSelectUI _characterSelectUI;
        private const string IdForScale = "scaleUp";

        private CharacterDataSo _characterDataSo;

        public void SelectCharacter()
        {
            _characterSelectUI.SetDetails(_characterDataSo);
        }

        public void SetCharacterDetails(CharacterDataSo data)
        {
            _characterDataSo = data;
        }
        
        public void SetCharacterSelectUI(CharacterSelectUI ui)
        {
            _characterSelectUI = ui;
        }
        
        #region Highlight

        public void OnPointerEnter(PointerEventData eventData)
        {
            Highlight();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StopHighlight();
        }
        
        private void Highlight()
        {
            DOTween.Complete(IdForScale);
            transform.DOScale(Vector3.one * 1.15f, 0.25f).SetId(IdForScale);
        }

        private void StopHighlight()
        {
            DOTween.Kill(IdForScale);
            transform.DOScale(Vector3.one, 0.25f).SetId(IdForScale);;
        }

        #endregion

        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
        }

        public void OnGet()
        {
        }
    }
}