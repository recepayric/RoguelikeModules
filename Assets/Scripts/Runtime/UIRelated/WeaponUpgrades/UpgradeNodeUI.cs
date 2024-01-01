using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UIRelated.WeaponUpgrades
{
    public class UpgradeNodeUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        public WeaponUpgradeScreenUI weaponUpgradeScreenUI;
        public UpgradeTreeNode upgradeTreeNode;
        public List<string> attributes;
        public string nodeName;

        public bool isBought = false;
        public Button btnBuy;

        public void SetStatus(bool status)
        {
            isBought = status;
            btnBuy.interactable = !isBought;
        }

        public void SetUpgradeTreeNode(UpgradeTreeNode pUpgradeTreeNode)
        {
            upgradeTreeNode = pUpgradeTreeNode;

            nodeName = upgradeTreeNode.NodeName;
            AddAttribute("•" + upgradeTreeNode.NodeText);
        }
        
        public void AddAttribute(string attribute)
        {
            attributes.Add(attribute);
           
        }

        public void BuyNode()
        {
            if (isBought) return;
            
            Debug.Log("Node bought!");
            isBought = true;
            btnBuy.interactable = false;
            weaponUpgradeScreenUI.ActivateNode(upgradeTreeNode);
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