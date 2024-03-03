using Data;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UIRelated.RuneRelated
{
    public class RuneInventorySlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPoolObject
    {
        public Sprite normalBackground;
        public Sprite magicBackground;
        public Sprite rareBackground;
        public Sprite epicBackground;
        public Sprite uniqueBackground;

        public Sprite[] runeImages;

        public Image backgroundImage;
        public Image runeImage;

        public Rune rune;

        public Vector2 targetAnchoredPosition;

        public void SetRune(Rune rune)
        {
            this.rune = rune;
            switch (rune.runeRarity)
            {
                case ItemRarity.Normal:
                    backgroundImage.sprite = normalBackground;
                    break;
                case ItemRarity.Magic:
                    backgroundImage.sprite = magicBackground;
                    break;
                case ItemRarity.Rare:
                    backgroundImage.sprite = rareBackground;
                    break;
                case ItemRarity.Epic:
                    backgroundImage.sprite = epicBackground;
                    break;
                case ItemRarity.Unique:
                    backgroundImage.sprite = uniqueBackground;
                    break;
            }

            if (rune.itemIcon == null)
                rune.itemIcon = runeImages[Random.Range(0, runeImages.Length)];

            runeImage.sprite = rune.itemIcon;
        }

        public void StopRaycastTarget()
        {
            backgroundImage.raycastTarget = false;
            runeImage.raycastTarget = false;
        }

        public void StartRaycastTarget()
        {
            backgroundImage.raycastTarget = true;
            runeImage.raycastTarget = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            EventManager.Instance.HoverRune(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            EventManager.Instance.CloseRuneDetails();
        }

        public PoolKeys PoolKeys { get; set; }

        public void OnReturn()
        {
        }

        public void OnGet()
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            Debug.Log("Dragging now!");
        }
    }
}