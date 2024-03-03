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
        public bool canBeBought = false;
        public Button btnBuy;

        public void SetStatus(bool status)
        {
            isBought = status;
            btnBuy.interactable = !isBought;
        }

        public void SetAvailable(bool status)
        {
            if (isBought) return;
            
            canBeBought = status;
            btnBuy.interactable = status;
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
            upgradeTreeNode.isBought = true;
            isBought = upgradeTreeNode.isBought;
            //isBought = true;
            btnBuy.interactable = false;
            weaponUpgradeScreenUI.ActivateNode(upgradeTreeNode);
            weaponUpgradeScreenUI.UpdateStatus();
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