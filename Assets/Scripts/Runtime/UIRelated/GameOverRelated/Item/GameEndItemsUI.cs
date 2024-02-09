using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.ItemsRelated;
using UnityEngine;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndItemsUI : MonoBehaviour
    {
        public GameEndItemDetailsUI itemDetails;
        public List<GameEndItemSlotUI> items;
        public ActiveCharacterSo ActiveCharacterSo;

        public void CreateItems()
        {
            ClearItems();

            for (int i = 0; i < ActiveCharacterSo.playerItems.Count; i++)
            {
                var itemSlot = BasicPool.instance.Get(PoolKeys.EndGameItemIcon);
                itemSlot.transform.localScale = Vector3.one;
                itemSlot.transform.localPosition = Vector3.zero;

                var sc = itemSlot.GetComponent<GameEndItemSlotUI>();
                items.Add(sc);

                sc.GameEndWeaponsUI = this;
                sc.SetItem(ActiveCharacterSo.playerItems[i]);
            }
            
        }

        private void ClearItems()
        {
            for (int i = 0; i < items.Count; i++)
            {
                BasicPool.instance.Return(items[i].gameObject);
            }
            items.Clear();
        }
        
        public void ShowItemDetail(Item item)
        {
            Debug.Log("Showing weapon deatilsd");
            itemDetails.gameObject.SetActive(true);
            itemDetails.ShowItemDetails(item);
        }

        public void HideWeapondetail()
        {
            itemDetails.gameObject.SetActive(false);
        }
    }
}