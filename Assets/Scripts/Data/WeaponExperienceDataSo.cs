using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "Weapons/Weapon Experience", order = 1)]
    public class WeaponExperienceDataSo : SerializedScriptableObject
    {
        [SerializeField] private float normalRarityNeededExperience;
        [SerializeField] private float magicRarityNeededExperience;
        [SerializeField] private float rareRarityNeededExperience;
        [SerializeField] private float epicRarityNeededExperience;
        [SerializeField] private float uniqueRarityNeededExperience;
        
        [Space(30)]
        [SerializeField] private float normalRarityExperienceIncreaseRate;
        [SerializeField] private float magicRarityExperienceIncreaseRate;
        [SerializeField] private float rareRarityExperienceIncreaseRate;
        [SerializeField] private float epicRarityExperienceIncreaseRate;
        [SerializeField] private float uniqueRarityExperienceIncreaseRate;
        
        public float NormalRarityNeededExperience => normalRarityNeededExperience;
        public float MagicRarityNeededExperience => magicRarityNeededExperience;
        public float RareRarityNeededExperience => rareRarityNeededExperience;
        public float EpicRarityNeededExperience => epicRarityNeededExperience;
        public float UniqueRarityNeededExperience => uniqueRarityNeededExperience;
        
        public float NormalRarityExperienceIncreaseRate => normalRarityExperienceIncreaseRate;
        public float MagicRarityExperienceIncreaseRate => magicRarityExperienceIncreaseRate;
        public float RareRarityExperienceIncreaseRate => rareRarityExperienceIncreaseRate;
        public float EpicRarityExperienceIncreaseRate => epicRarityExperienceIncreaseRate;
        public float UniqueRarityExperienceIncreaseRate => uniqueRarityExperienceIncreaseRate;
    }
}