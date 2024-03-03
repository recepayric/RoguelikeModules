using System;
using System.Collections.Generic;
using Runtime;
using Runtime.Enums;
using Runtime.Modifiers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SkillData", order = 1)]
    public class WeaponSkillDataSo : SerializedScriptableObject
    {
        public int maxTier;
        public Dictionary<SpecialModifiers, WeaponSkillData> weaponSkills;

        [Button]
        public void GetAllSkills()
        {
            var PieceTypeNames = Enum.GetValues(typeof(SpecialModifiers));

            int skillIndex = 1;
            for (int i = 0; i < PieceTypeNames.Length; i++)
            {
                var key = (SpecialModifiers)PieceTypeNames.GetValue(i);
                var modifier = ModifierCreator.GetModifier(key);


                if (modifier.isWeaponSkill && !weaponSkills.ContainsKey(key))
                {
                    var skillData = new WeaponSkillData();
                    skillData.effectPerTier = new float[maxTier];
                    skillData.secondEffectPerTier = new float[maxTier];
                    skillData.thirdEffectPerTier = new float[maxTier];
                    skillData.numberOfSkillPerTier = new int[maxTier];
                    skillData.numberOfSkillsUsedPerTier = new int[maxTier];
                    skillData.skillNumber = skillIndex;
                    skillData.modifierName = key;
                    weaponSkills.Add(key, skillData);
                    skillIndex++;
                }
            }
        }

        public void Reset()
        {
            foreach (var skill in weaponSkills)
            {
                skill.Value.numberOfSkillsUsedPerTier = new int[maxTier];
                skill.Value.slotData.Clear();
            }
        }

        public int GetTier(Weapon weapon, SpecialModifiers specialModifiers)
        {
            var skillData = weaponSkills[specialModifiers];
            for (int i = 0; i < skillData.slotData.Count; i++)
            {
                if (skillData.slotData[i].weapon == weapon)
                    return skillData.slotData[i].tier;
            }

            return -1;
        }

        public WeaponSkillData GetEffect(SpecialModifiers specialModifiers)
        {
            if (!weaponSkills.ContainsKey(specialModifiers)) return null;
            return weaponSkills[specialModifiers];
        }
    }

    [Serializable]
    public class WeaponSkillData
    {
        public int tier;
        public SpecialModifiers modifierName;
        public float[] effectPerTier;
        public float[] secondEffectPerTier;
        public float[] thirdEffectPerTier;
        public int[] numberOfSkillPerTier;
        public int[] numberOfSkillsUsedPerTier;
        public Sprite skillIcon;
        public int skillNumber;
        public bool isEquipped;
        public Dictionary<GameObject, int> weaponAndTiers;
        public List<SkillSlotData> slotData = new List<SkillSlotData>();

        public bool GetCanAdd(GameObject weapon, int tier)
        {
            for (int i = 0; i < slotData.Count; i++)
            {
                if (slotData[i].weapon == weapon && slotData[i].modifierName == modifierName) return false;
            }

            return true;
        }

        public void AddWeapon(GameObject weaponObject, int tier)
        {
            slotData.Add(new SkillSlotData(weaponObject, tier, modifierName));
        }

        public void RemoveWeapon(GameObject weapon, int tier)
        {
            for (int i = 0; i < slotData.Count; i++)
            {
                if (slotData[i].weapon == weapon && slotData[i].tier == tier &&
                    slotData[i].modifierName == modifierName)
                {
                    slotData.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class SkillSlotData
    {
        public GameObject weapon;
        public int tier;
        public SpecialModifiers modifierName;

        public SkillSlotData(GameObject _weapon, int _tier, SpecialModifiers _modifierName)
        {
            weapon = _weapon;
            tier = _tier;
            modifierName = _modifierName;
        }
    }
}