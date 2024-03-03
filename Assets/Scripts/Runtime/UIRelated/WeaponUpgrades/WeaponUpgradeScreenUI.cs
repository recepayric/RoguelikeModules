using System;
using System.Collections.Generic;
using Data;
using Runtime.Managers;
using Runtime.UIRelated.WeaponUpgrades.WeaponSkill;
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
        public WeaponUpgradeSkillListUI weaponUpgradeSkillListUI;
        public WeaponSkillDetailPanelUI weaponSkillDetailPanelUI;
        public List<GameObject> upgradeNodes;
        public List<UpgradeNodeUI> upgradeNodeScripts;
        public List<WeaponSkillSlotUI> skillSlots;

        public NodeDetailsPanel sNodeDetailPanel;
        public GameObject nodeDetailPanel;
        public GameObject skillListPanel;
        public bool isDetailPanelOpen;

        public void OpenNodeDetail(UpgradeNodeUI node)
        {
            isDetailPanelOpen = true;

            //todo set details!
            sNodeDetailPanel.SetName(node.nodeName);
            sNodeDetailPanel.AddAttribute(node.attributes[0]);
            nodeDetailPanel.SetActive(true);
        }

        public void OpenSkillList(WeaponSkillSlotUI skillSlot)
        {
            skillListPanel.SetActive(true);
            weaponUpgradeSkillListUI.SetAvailableSkills(skillSlot, weaponUpgradeTree.gameObject);
        }

        public void OpenSkillDetail(WeaponSkillData skillData)
        {
            weaponSkillDetailPanelUI.gameObject.SetActive(true);
            weaponSkillDetailPanelUI.SetDetails(skillData);
        }

        public void CloseSkillDetail()
        {
            weaponSkillDetailPanelUI.gameObject.SetActive(false);
        }

        public void CloseNodeDetail()
        {
            isDetailPanelOpen = false;
            sNodeDetailPanel.Clear();
            nodeDetailPanel.SetActive(false);
        }

        public void EquipSkill(WeaponSkillSlotUI skillSlot, WeaponUpgradeSkillUI skillToEquip)
        {
            var equippedSkill = skillSlot.equippedSkill;
            var slotNumber = skillSlots.IndexOf(skillSlot);

            if (skillSlot.isSlotFull)
            {
                equippedSkill.skillData.numberOfSkillsUsedPerTier[equippedSkill.tier]--;
                weaponUpgradeTree.RemoveSkill(equippedSkill.skillData, equippedSkill.tier, slotNumber);
            }

            skillSlot.EquipSkill(skillToEquip.skillData, skillToEquip.tier);
            skillToEquip.skillData.numberOfSkillsUsedPerTier[skillToEquip.tier]++;
            weaponUpgradeTree.EquipSkill(skillToEquip.skillData, skillToEquip.tier, slotNumber);
            weaponUpgradeSkillListUI.SetAvailableSkills(skillSlot, weaponUpgradeTree.gameObject);
        }

        public void UnEquipSkill(WeaponSkillSlotUI skillSlot, WeaponUpgradeSkillUI skillToUnEquip)
        {
            var equippedSkill = skillSlot.equippedSkill;
            var slotNumber = skillSlots.IndexOf(skillSlot);
            equippedSkill.skillData.numberOfSkillsUsedPerTier[equippedSkill.tier]--;
            skillSlot.UnEquipSkill();
            weaponUpgradeTree.RemoveSkill(equippedSkill.skillData, equippedSkill.tier, slotNumber);
            weaponUpgradeSkillListUI.SetAvailableSkills(skillSlot, weaponUpgradeTree.gameObject);
        }

        public void GetEquippedSkills()
        {
            return;
            foreach (var skill in weaponUpgradeTree.equippedSkills)
            {
                var slot = skill.Key;
                //skillSlots[slot].EquipSkill(skill.Value);
            }
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
                //upgradeNodeScripts[i].SetStatus(weaponUpgradeTree.CheckIfNodeActive(weaponUpgradeTreeSo.TreeNodes[i]));
                upgradeNodeScripts[i].SetStatus(weaponUpgradeTreeSo.TreeNodes[i].isBought);
            }
            
            for (int i = 0; i < weaponUpgradeTreeSo.SkillSlotNodes.Count; i++)
            {
                //upgradeNodeScripts[i].SetStatus(weaponUpgradeTree.CheckIfNodeActive(weaponUpgradeTreeSo.TreeNodes[i]));
                skillSlots[i].SetStatus(weaponUpgradeTreeSo.SkillSlotNodes[i].isBought);
            }

            for (int i = nodeCount; i < upgradeNodeScripts.Count; i++)
            {
                upgradeNodeScripts[i].nodeName = Random.Range(100, 200).ToString();
                upgradeNodeScripts[i].AddAttribute("•" + Random.Range(100, 200).ToString());
            }
            
            UpdateStatus();
            GetEquippedSkills();
        }

        public void UpdateStatus()
        {
            for (int i = 0; i < weaponUpgradeTreeSo.TreeNodes.Count; i++)
            {
                var setNumber = i % 3;
                if (setNumber == 0)
                {
                    upgradeNodeScripts[i].SetStatus(weaponUpgradeTreeSo.TreeNodes[i].isBought);
                }
                else
                {
                    upgradeNodeScripts[i].SetAvailable(weaponUpgradeTreeSo.TreeNodes[i-1].isBought);
                }
            }
            
            for (int i = 0; i < weaponUpgradeTreeSo.SkillSlotNodes.Count; i++)
            {
                var setNumber = (i+1)*3-1;
                skillSlots[i].SetAvailability(weaponUpgradeTreeSo.TreeNodes[setNumber].isBought);
            }
        }
        public bool CanBuy(UpgradeNodeUI node)
        {
            
            return false;
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
            
            Debug.Log("Setting nodes!!!!!");
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
            GetEquippedSkills();
        }

        public void OnClosed()
        {
            RemoveEvents();
        }
    }
}