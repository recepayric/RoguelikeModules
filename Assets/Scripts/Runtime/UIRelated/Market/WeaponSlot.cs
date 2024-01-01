using Data;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.Market
{
    public class WeaponSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Weapon weapon;
        public WeaponDatasSo _weaponDatasSo;
        public MarketUI marketUI;
        
        public void OpenWeaponSkillTree()
        {
            EventManager.Instance.OpenScreen(Screens.WeaponUpgrade, true);
            EventManager.Instance.SetWeaponDataForTree(weapon.weaponUpgradeTree.weaponUpgradeTree);
            EventManager.Instance.SetWeaponForTree(weapon);
        }

        public bool HasUnusedSkillPoint()
        {
            return false;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            marketUI.SetWeaponDetails(_weaponDatasSo);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            marketUI.SetWeaponDetails(null);
        }
    }
}