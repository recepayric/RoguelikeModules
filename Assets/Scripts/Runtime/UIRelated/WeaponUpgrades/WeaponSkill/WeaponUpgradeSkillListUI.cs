using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

namespace Runtime.UIRelated.WeaponUpgrades.WeaponSkill
{
    public class WeaponUpgradeSkillListUI : MonoBehaviour
    {
        public WeaponUpgradeScreenUI weaponUpgradeScreenUI;
        public WeaponSkillDataSo skillDataSo;
        public WeaponSkillSlotUI weaponSkillSlot;
        public GameObject skillContent;
        public GameObject skillPrefab;

        private List<WeaponUpgradeSkillUI> _skillsToSelect;

        public void SetAvailableSkills(WeaponSkillSlotUI pWeaponSkillSlot, GameObject weapon)
        {
            DeactivateAllSkills();
            weaponSkillSlot = pWeaponSkillSlot;
            var list = skillDataSo.weaponSkills.Keys.ToList();
            int listIndex = 0;
            for (int i = 0; i < list.Count; i++)
            {
                var skillData = skillDataSo.weaponSkills[list[i]];
                var tier = skillData.tier;

                //if (tier < 1) continue;
                //if (skillData.isEquipped) continue;

                for (int j = 0; j < skillData.numberOfSkillPerTier.Length; j++)
                {
                    var remainingSkillCount =
                        skillData.numberOfSkillPerTier[j] - skillData.numberOfSkillsUsedPerTier[j];
                    var canAdd = skillData.GetCanAdd(weapon, j);
                    if (remainingSkillCount == 0 || !canAdd)
                    {
                        continue;
                    }

                    if (_skillsToSelect.Count > listIndex)
                    {
                        _skillsToSelect[listIndex].gameObject.SetActive(true);
                        _skillsToSelect[listIndex].transform.SetParent(skillContent.transform);
                        _skillsToSelect[listIndex].SetSkill(skillData, j);
                        _skillsToSelect[listIndex].weaponSkillSlotUI = weaponSkillSlot;

                    }
                    else
                    {
                        var obj = Instantiate(skillPrefab, skillContent.transform);
                        var data = obj.GetComponent<WeaponUpgradeSkillUI>();
                        data.SetSkill(skillData, j);
                        data.weaponUpgradeScreenUI = weaponUpgradeScreenUI;
                        data.weaponSkillSlotUI = weaponSkillSlot;
                        _skillsToSelect.Add(data);
                    }

                    listIndex++;
                }
            }
        }

        private void DeactivateAllSkills()
        {
            if (_skillsToSelect == null)
                _skillsToSelect = new List<WeaponUpgradeSkillUI>();
            for (int i = 0; i < _skillsToSelect.Count; i++)
            {
                _skillsToSelect[i].gameObject.SetActive(false);
            }
        }
    }
}