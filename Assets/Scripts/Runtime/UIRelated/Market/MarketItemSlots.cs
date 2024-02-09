using System;
using Data.WeaponDataRelated;
using DG.Tweening;
using Runtime.Configs;
using Runtime.ItemsRelated;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UIRelated.Market
{
    public class MarketItemSlots : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public bool isItem = false;
        public MarketUI marketUI;
        public GameObject backgroundImage;
        public Image itemIcon;
        public float turnDirection;
        public float turnSpeed;
        public Vector3 turnSpeedVector;
        public Item item;

        public string ScaleId;

        public bool canBeBought = false;

        public void Start()
        {
            ScaleId = "Scale" + GetInstanceID();
            turnSpeedVector = new Vector3(0, 0, turnSpeed * turnDirection);
        }

        public void Update()
        {
            backgroundImage.transform.Rotate(turnSpeedVector * Time.deltaTime);
        }

        public void SetItem(Item pItem)
        {
            canBeBought = false;
            isItem = true;
            DOTween.Kill(ScaleId);

            itemIcon.gameObject.transform
                .DOScale(new Vector3(0, 0, 0), AnimationConfig.MarketResetItemAnimationTime / 2)
                .SetId(ScaleId).OnComplete(() =>
                {
                    item = pItem;
                    SetIcon();
                });
        }

        public void SetItem(WeaponDataSo weaponDataSo)
        {
            canBeBought = false;
            isItem = false;
            DOTween.Kill(ScaleId);

            itemIcon.gameObject.transform
                .DOScale(new Vector3(0, 0, 0), AnimationConfig.MarketResetItemAnimationTime / 2)
                .SetId(ScaleId).OnComplete(() =>
                {
                    //item = pItem;
                    SetIcon();
                });
        }

        public void SetIcon()
        {
            itemIcon.sprite = item.itemIcon;
            itemIcon.gameObject.transform
                .DOScale(new Vector3(1, 1, 1), AnimationConfig.MarketResetItemAnimationTime / 2)
                .SetId(ScaleId).OnComplete(() => { canBeBought = true; });
        }

        public void BuyItem()
        {
            if (!canBeBought) return;
            EventManager.Instance.ItemBuy(item);
            marketUI.BuyItem(item);
            RemoveItem();
        }

        private void RemoveItem()
        {
            canBeBought = false;
            itemIcon.gameObject.transform
                .DOScale(new Vector3(0, 0, 0), AnimationConfig.MarketResetItemAnimationTime / 2)
                .SetId(ScaleId);
            marketUI.StopShowingItemDetails();
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            if(canBeBought)
                marketUI.SetItemDetails(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            marketUI.StopShowingItemDetails();
        }
    }
}