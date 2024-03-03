using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StatTierDetails", order = 1)]
    public class StatTierDetails : SerializedScriptableObject
    {
        //public Dictionary<AllStats, List<StatValueForStatData>> statValues;
        public Dictionary<AllStats, Dictionary<int, StatValueForStatData>> statValues2;

        public List<AllStats> statsToAssign;
        public int statTierNumber;
        [Button]
        public void AssignValues()
        {
            statValues2.Clear();

            for (int i = 0; i < statsToAssign.Count; i++)
            {
                statValues2.Add(statsToAssign[i], new Dictionary<int, StatValueForStatData>());
                for (int j = 0; j < statTierNumber; j++)
                {
                    statValues2[statsToAssign[i]].Add(j, new StatValueForStatData());
                    statValues2[statsToAssign[i]][j].tier = j;

                }
            }
        }
    }

    public class StatValueForStatData
    {
        public int tier;
        public int minStat;
        public int maxStat;
    }
}