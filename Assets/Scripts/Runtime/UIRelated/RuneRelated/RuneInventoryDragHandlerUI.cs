using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Configs;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.RuneRelated
{
    public class RuneInventoryDragHandlerUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        private RuneInventorySlotUI _runeInventorySlotUI;
        private RectTransform _rectTransform;
        private Vector2 dragAmount;


        private void Start()
        {
            _runeInventorySlotUI = GetComponent<RuneInventorySlotUI>();
            _rectTransform = GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            dragAmount += eventData.delta;
            _rectTransform.anchoredPosition += eventData.delta;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _runeInventorySlotUI.StopRaycastTarget();
            Debug.Log("Drag Started: " + gameObject.GetInstanceID());
            dragAmount = Vector2.zero;
            var currentParent = transform.parent;

            var tempParent = transform.parent.parent;
            transform.SetParent(tempParent);
            transform.SetParent(currentParent);

            EventManager.Instance.RuneDragStart();
            ScaleUp();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _runeInventorySlotUI.StartRaycastTarget();
            Debug.Log("Drag Ended: " + gameObject.GetInstanceID());
            if (HasTargetSlot())
                PlaceRuneToSlot();
            else
                ResetRunePosition();
        }

        private void ScaleUp()
        {
            DOTween.Kill(_runeInventorySlotUI.rune.rune_id);
            //transform.DOScale(UIConfig.RuneScaleUpAmount * Vector3.one, UIConfig.RuneScaleUpTime)
            //    .SetId(_runeInventorySlotUI.rune.rune_id);

            _rectTransform.DOSizeDelta(new Vector2(150, 150), UIConfig.RuneScaleUpTime).SetId(_runeInventorySlotUI.rune.rune_id);
        }

        private void ScaleDown()
        {
            DOTween.Kill(_runeInventorySlotUI.rune.rune_id);
            _rectTransform.DOSizeDelta(new Vector2(80, 80), UIConfig.RuneScaleUpTime).SetId(_runeInventorySlotUI.rune.rune_id);
            //transform.DOScale(Vector3.one, UIConfig.RuneScaleUpTime)
            //    .SetId(_runeInventorySlotUI.rune.rune_id);
        }

        private void PlaceRuneToSlot()
        {
        }

        private void ResetRunePosition()
        {
            _rectTransform.anchoredPosition -= dragAmount;
            EventManager.Instance.RuneDragCancelled();
            ScaleDown();
        }

        private bool HasTargetSlot()
        {
            return false;
        }
    }
}