using Data;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.WeaponRelated
{
    public class WeaponLevelSystem : MonoBehaviour
    {

        private WeaponExperienceDataSo _weaponExperienceData;
        private ItemRarity _weaponRarity;
        
        //Stats
        public float BaseNeededExperience;
        public float BaseExperienceIncreaseRate;
        
        //Weapon Upgrade Tree
        public WeaponUpgradeTree weaponUpgradeTree;
        
        //Exp
        public float currentExperience;
        public float requiredExperience;
        
        //Level
        public int weaponLevel = 1; //starts with 1, no upgrade points.
        
        
        public void Start()
        {
            _weaponExperienceData = Resources.Load<WeaponExperienceDataSo>("WeaponExperienceConfig");
            GetBaseExperienceRequirements();
            CalculateNeededExperience();
            weaponLevel = 1;
        }

        [Button]
        public void AddExperience(float experienceAmount)
        {
            currentExperience += experienceAmount;
            CheckForLevelUp();
        }

        private void CalculateNeededExperience()
        {
            requiredExperience = BaseNeededExperience * Mathf.Pow(BaseExperienceIncreaseRate, weaponLevel - 1);
        }

        //todo save these
        private void CheckForLevelUp()
        {
            while (currentExperience >= requiredExperience)
            {
                LevelUp();
            }
        }

        private void LevelUp()
        {
            currentExperience -= requiredExperience;
            weaponLevel++;
            CalculateNeededExperience();
            weaponUpgradeTree.AddStatPoint(1);
        }
        
        private void GetBaseExperienceRequirements()
        {
            switch (_weaponRarity)
            {
                case ItemRarity.Normal:
                    BaseNeededExperience = _weaponExperienceData.NormalRarityNeededExperience;
                    BaseExperienceIncreaseRate = _weaponExperienceData.NormalRarityExperienceIncreaseRate;
                    break;
                case ItemRarity.Magic:
                    BaseNeededExperience = _weaponExperienceData.MagicRarityNeededExperience;
                    BaseExperienceIncreaseRate = _weaponExperienceData.MagicRarityExperienceIncreaseRate;
                    break;
                case ItemRarity.Rare:
                    BaseNeededExperience = _weaponExperienceData.RareRarityNeededExperience;
                    BaseExperienceIncreaseRate = _weaponExperienceData.RareRarityExperienceIncreaseRate;
                    break;
                case ItemRarity.Epic:
                    BaseNeededExperience = _weaponExperienceData.EpicRarityNeededExperience;
                    BaseExperienceIncreaseRate = _weaponExperienceData.EpicRarityExperienceIncreaseRate;
                    break;
                case ItemRarity.Unique:
                    BaseNeededExperience = _weaponExperienceData.UniqueRarityNeededExperience;
                    BaseExperienceIncreaseRate = _weaponExperienceData.UniqueRarityExperienceIncreaseRate;
                    break;
            }
        }
    }
}