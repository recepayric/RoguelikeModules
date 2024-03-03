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
        public WeaponDataSo weapon;

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
            weapon = weaponDataSo;
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
            if (isItem)
                itemIcon.sprite = item.itemIcon;
            else
                itemIcon.sprite = weapon.waaponSprite;

            itemIcon.gameObject.transform
                .DOScale(new Vector3(1, 1, 1), AnimationConfig.MarketResetItemAnimationTime / 2)
                .SetId(ScaleId).OnComplete(() => { canBeBought = true; });
        }

        public void BuyItem()
        {
            if (!canBeBought) return;
            if (isItem)
            {
                EventManager.Instance.ItemBuy(item);
                marketUI.BuyItem(item);    
            }
            else
            {
                EventManager.Instance.WeaponBuy(weapon.WeaponPoolKey);
                marketUI.BuyWeapon();
            }
            
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
            if (canBeBought)
            {
                if(isItem)
                    marketUI.SetItemDetails(item);
                else
                    marketUI.SetWeaponDetails(weapon);
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            marketUI.StopShowingItemDetails();
        }
    }
}