using Runtime.Enums;
using Runtime.ItemsRelated;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndItemSlotUI : MonoBehaviour, IPoolObject, IPointerEnterHandler, IPointerExitHandler
    {
        
        public GameEndItemsUI GameEndWeaponsUI;
        public Image itemImage;
        public Item item;
        public TextMeshProUGUI itemCount;

        public void SetItem(Item pItem)
        {
            item = pItem;
            itemImage.sprite = pItem.itemIcon;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            GameEndWeaponsUI.ShowItemDetail(item);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEndWeaponsUI.HideWeapondetail();
        }
        
        public PoolKeys PoolKeys { get; set; }
        public void OnReturn()
        {
        }

        public void OnGet()
        {
        }
    }
}