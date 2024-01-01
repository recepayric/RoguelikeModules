using System;
using System.Collections.Generic;
using Data;
using Runtime.Managers;
using Runtime.WeaponRelated;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Runtime.UIRelated.WeaponUpgrades
{
    public class WeaponUpgradeScreenUI : MonoBehaviour, IOpenable
    {
        public Weapon weapon;
        public WeaponUpgradeTreeSo weaponUpgradeTreeSo;
        public WeaponUpgradeTree weaponUpgradeTree;
        
        public List<GameObject> upgradeNodes;
        public List<UpgradeNodeUI> upgradeNodeScripts;

        public NodeDetailsPanel sNodeDetailPanel;
        public GameObject nodeDetailPanel;
        public bool isDetailPanelOpen;

        public void OpenNodeDetail(UpgradeNodeUI node)
        {
            isDetailPanelOpen = true;
            
            //todo set details!
            sNodeDetailPanel.SetName(node.nodeName);
            sNodeDetailPanel.AddAttribute(node.attributes[0]);
            nodeDetailPanel.SetActive(true);
        }
        
        
        public void CloseNodeDetail()
        {
            isDetailPanelOpen = false;
            sNodeDetailPanel.Clear();
            nodeDetailPanel.SetActive(false);
        }

        [Button]
        public void GetScripts()
        {
            upgradeNodeScripts.Clear();
            
            for (int i = 0; i < upgradeNodes.Count; i++)
            {
                upgradeNodeScripts.Add(upgradeNodes[i].GetComponent<UpgradeNodeUI>());
            }

            for (int i = 0; i < upgradeNodeScripts.Count; i++)
            {
                upgradeNodeScripts[i].weaponUpgradeScreenUI = this;
            }
        }

        [Button]
        public void SetNodes()
        {
            var nodeCount = weaponUpgradeTreeSo.TreeNodes.Count;
            for (int i = 0; i < weaponUpgradeTreeSo.TreeNodes.Count; i++)
            {
                upgradeNodeScripts[i].SetUpgradeTreeNode(weaponUpgradeTreeSo.TreeNodes[i]);
                upgradeNodeScripts[i].SetStatus(weaponUpgradeTree.CheckIfNodeActive(weaponUpgradeTreeSo.TreeNodes[i]));
            }

            for (int i = nodeCount; i < upgradeNodeScripts.Count; i++)
            {
                upgradeNodeScripts[i].nodeName = Random.Range(100, 200).ToString();
                upgradeNodeScripts[i].AddAttribute("•" + Random.Range(100, 200).ToString());
            }
        }

        public void ActivateNode(UpgradeTreeNode nodeToActivate)
        {
            weaponUpgradeTree.ActivateNode(nodeToActivate);
        }

        private void OnSetWeaponDataForTree(WeaponUpgradeTreeSo weaponTreeData)
        {
            //weaponUpgradeTreeSo = weaponTreeData;
            //SetNodes();
        }

        private void OnSetWeaponForTree(Weapon pWeapon)
        {
            weapon = pWeapon;
            weaponUpgradeTreeSo = weapon.weaponUpgradeTree.weaponUpgradeTree;
            weaponUpgradeTree = weapon.weaponUpgradeTree;
            SetNodes();
        }

        private void AddEvents()
        {
            EventManager.Instance.SetWeaponDataForTreeEvent += OnSetWeaponDataForTree;
            EventManager.Instance.SetWeaponForTreeEvent += OnSetWeaponForTree;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.SetWeaponDataForTreeEvent -= OnSetWeaponDataForTree;
            EventManager.Instance.SetWeaponForTreeEvent -= OnSetWeaponForTree;
        }

        public void OnOpened()
        {
            AddEvents();
        }

        public void OnClosed()
        {
            RemoveEvents();
        }
    }
}