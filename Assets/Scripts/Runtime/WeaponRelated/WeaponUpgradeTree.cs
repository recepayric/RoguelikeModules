using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.WeaponRelated
{
    public class WeaponUpgradeTree : MonoBehaviour
    {
        public Weapon weapon;
        public WeaponUpgradeTreeSo weaponUpgradeTree;
        public List<UpgradeTreeNode> activatedTreeNodes;
        public Dictionary<int, WeaponSkillData> equippedSkills = new Dictionary<int, WeaponSkillData>();
        public int freeStatPoints;
        public int totalStatPoints;

        //Types = melee, ranged, magic, trap
        //Change their element or make them elemental!
        //Upgrade their basic stats like attack speed, range, damage...
        //Change their appearance

        public void EquipSkill(WeaponSkillData skillData, int tier, int skillSlot)
        {
            Debug.Log("skillSlot"+skillSlot);
            skillData.AddWeapon(gameObject, tier);
            equippedSkills.Add(skillSlot, skillData);
            weapon.AddModifierFromTree(skillData.modifierName);
        }

        public void RemoveSkill(WeaponSkillData skillData,  int tier, int skillSlot)
        {
            skillData.RemoveWeapon(gameObject, tier);
            equippedSkills.Remove(skillSlot);
            weapon.RemoveModifierFromTree(skillData.modifierName);
        }

        //Has requirement upgrade

        private void Start()
        {
            weapon = GetComponent<Weapon>();
            for (int i = 0; i < weaponUpgradeTree.TreeNodes.Count; i++)
            {
                weaponUpgradeTree.TreeNodes[i].ResetNode();
            }
        }

        public void ActivateNode(UpgradeTreeNode upgradeTreeNode)
        {
            if (upgradeTreeNode.IsActive) return;
            
            activatedTreeNodes.Add(upgradeTreeNode);

            var upgradeType = upgradeTreeNode.UpgradeType;

            if (upgradeType == UpgradeType.Stats)
                weapon.AddStatFromTree(upgradeTreeNode.Stat, upgradeTreeNode.IncreaseAmount);
            else if (upgradeType == UpgradeType.SpecialModifier)
                weapon.AddModifierFromTree(upgradeTreeNode.Modifier);
            
            if (upgradeTreeNode.doesAddStyle)
            {
                weapon.AddStyle(upgradeTreeNode.styleNumber);
            }
            
            upgradeTreeNode.ActivateNode();
        }

        public bool CheckIfNodeActive(UpgradeTreeNode nodeToCheck)
        {
            return activatedTreeNodes.Contains(nodeToCheck);
        }

        [Button]
        public void RefreshTree()
        {
        }

        public void AddStatPoint(int amount)
        {
            freeStatPoints += amount;
        }

        public void SpentPoint()
        {
            freeStatPoints--;
        }

        public void ResetTree()
        {
            weaponUpgradeTree.Reset();
            activatedTreeNodes.Clear();
        }
    }
}