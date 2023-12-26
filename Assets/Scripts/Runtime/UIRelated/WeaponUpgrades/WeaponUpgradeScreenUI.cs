using System;
using System.Collections.Generic;
using Data;
using Runtime.Managers;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace Runtime.UIRelated.WeaponUpgrades
{
    public class WeaponUpgradeScreenUI : MonoBehaviour, IOpenable
    {
        public WeaponUpgradeTreeSo weaponUpgradeTree;
        
        public List<GameObject> upgradeNodes;
        public List<UpgradeNodeUI> upgradeNodeScripts;

        public NodeDetailsPanel sNodeDetailPanel;
        public GameObject nodeDetailPanel;
        public bool isDetailPanelOpen;
        
        private void Start()
        {
            
        }

        private void Awake()
        {
            
        }

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
            var nodeCount = weaponUpgradeTree.TreeNodes.Count;
            for (int i = 0; i < weaponUpgradeTree.TreeNodes.Count; i++)
            {
                upgradeNodeScripts[i].nodeName = weaponUpgradeTree.TreeNodes[i].NodeName;
                upgradeNodeScripts[i].AddAttribute("•" + weaponUpgradeTree.TreeNodes[i].NodeText);
            }

            for (int i = nodeCount; i < upgradeNodeScripts.Count; i++)
            {
                upgradeNodeScripts[i].nodeName = Random.Range(100, 200).ToString();
                upgradeNodeScripts[i].AddAttribute("•" + Random.Range(100, 200).ToString());
            }
        }

        private void OnSetWeaponDataForTree(WeaponUpgradeTreeSo weaponTreeData)
        {
            weaponUpgradeTree = weaponTreeData;
            SetNodes();
        }

        private void AddEvents()
        {
            EventManager.Instance.SetWeaponDataForTreeEvent += OnSetWeaponDataForTree;
        }

        private void RemoveEvents()
        {
            EventManager.Instance.SetWeaponDataForTreeEvent -= OnSetWeaponDataForTree;
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