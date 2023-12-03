using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.WeaponUpgrades
{
    public class UpgradeNodeUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        public WeaponUpgradeScreenUI weaponUpgradeScreenUI;
        
        public List<string> attributes;
        public string nodeName;

        public void AddAttribute(string attribute)
        {
            attributes.Add(attribute);
        }
        
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            weaponUpgradeScreenUI.OpenNodeDetail(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            weaponUpgradeScreenUI.CloseNodeDetail();
        }
    }
}