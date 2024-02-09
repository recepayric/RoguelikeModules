using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndWeaponSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameEndWeaponsUI GameEndWeaponsUI;
        public Image WeaponImage;
        public Weapon Weapon;

        public void SetWeapon(Weapon weapon)
        {
            Weapon = weapon;
            WeaponImage.sprite = weapon.weaponDataSo.waaponSprite;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            GameEndWeaponsUI.ShowWeaponDetail(Weapon);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            GameEndWeaponsUI.HideWeapondetail();
        }
    }
}