using System.Collections.Generic;
using Data;
using Runtime.ItemsRelated;
using UnityEngine;

namespace Runtime.UIRelated.GameOverRelated.Weapons
{
    public class GameEndWeaponsUI : MonoBehaviour
    {
        public GameEndItemDetailsUI itemDetails;
        public List<GameEndWeaponSlotUI> WeaponSlots;
        public ActiveCharacterSo ActiveCharacterSo;

        public void SetWeapons()
        {
            for (int i = 0; i < WeaponSlots.Count; i++)
            {
                var hasWeapon = ActiveCharacterSo.playerWeapons.Count > i;
                if (!hasWeapon)
                {
                    WeaponSlots[i].gameObject.SetActive(false);
                    continue;
                }
                
                WeaponSlots[i].gameObject.SetActive(true);
                WeaponSlots[i].GameEndWeaponsUI = this;
                WeaponSlots[i].SetWeapon(ActiveCharacterSo.playerWeapons[i]);
            }
        }

        public void ShowWeaponDetail(Weapon weapon)
        {
            itemDetails.gameObject.SetActive(true);
            itemDetails.ShowWeaponDetails(weapon);
        }

        public void ShowItemDetail(Item item)
        {
            itemDetails.gameObject.SetActive(true);
            itemDetails.ShowItemDetails(item);
        }
        
        public void HideWeapondetail()
        {
            itemDetails.gameObject.SetActive(false);
        }

    }
}