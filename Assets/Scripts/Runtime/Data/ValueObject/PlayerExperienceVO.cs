using System;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public class PlayerExperienceVO
    {
        public int currentLevel;
        public int statPoint;
        public float requiredExperience;
        public float baseRequiredExperience;
        public float currentExperience;
        public float requiredExperienceIncrease;

        public void Reset()
        {
            currentExperience = 0;
            currentLevel = 0;
            statPoint = 0;
        }

        public void AddExperience(float exp)
        {
            currentExperience += exp;
        }

        public void LevelUp()
        {
            currentLevel++;
            statPoint++;
        }

        public void UsePoint()
        {
            statPoint--;
        }

        public void SetRequiredExperience(float experience)
        {
            requiredExperience = experience;
        }
    }
}