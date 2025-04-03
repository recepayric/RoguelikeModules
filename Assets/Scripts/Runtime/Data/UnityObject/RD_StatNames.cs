using System;
using System.Collections.Generic;
using CodeStage.AntiCheat.ObscuredTypes;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "ArvaveGames/Stat/Stat Names", order = 0)]
    public class RD_StatNames : SerializedScriptableObject
    {
        public int currentIndex;
        public List<StatName> statNames;
        public StatName selectedStat;
        public Dictionary<ItemModifiers, string> ModifierTexts;
        public Color32 colorPositiveStat;
        public Color32 colorNegativeStat;
        public Color32 colorRegularColor;
        public string colorCode;
        
        public string GetStatName(AllStats stat)
        {
            var statNameClass = statNames.Find(t => t.stat == stat);

            if (statNameClass == null)
                return stat.ToString();
            
            return statNameClass.statName;
        }

        public string GetStatValue(AllStats stat, ObscuredFloat value, bool includePlus = false)
        {
            var statNameClass = statNames.Find(t => t.stat == stat);
            var extra = "";
            
            if (statNameClass != null)
                extra = statNameClass.statShowType == StatShowType.Percentage ? "%" : "";

            
            string fullText = "";
            
            if (value > 0f)
            {
                colorCode = "#"+ColorUtility.ToHtmlStringRGB(colorPositiveStat);

                if(includePlus)
                    fullText += "<color="+colorCode+">+"+value+extra+"</color>";
                else
                    fullText += "<color="+colorCode+">"+value+extra+"</color>";
            }
            else if (value < 0f)
            {
                colorCode = "#"+ColorUtility.ToHtmlStringRGB(colorNegativeStat);
                fullText += "<color="+colorCode+">"+value+extra+"</color>";
            }
            else
            {
                colorCode = "#"+ColorUtility.ToHtmlStringRGB(colorRegularColor);
                fullText += "<color="+colorCode+">"+value+extra+"</color>";
            }

            return fullText;
        }
        

        [Button]
        public void NextStat()
        {
            currentIndex++;
            if (currentIndex >= statNames.Count)
                currentIndex = 0;
            
            selectedStat = statNames[currentIndex];
        }
        
        [Button]
        public void PreviousStat()
        {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = statNames.Count-1;
            
            selectedStat = statNames[currentIndex];
        }
    }

    [Serializable]
    public class StatName
    {
        public AllStats stat;
        public StatShowType statShowType;
        public string statName;
        public string statNameUs;
    }
}