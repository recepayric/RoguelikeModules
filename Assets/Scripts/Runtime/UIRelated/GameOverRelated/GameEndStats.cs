using System.Collections.Generic;
using System.Text.RegularExpressions;
using Runtime.Enums;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndStats : MonoBehaviour
    {
        public List<AllStats> statsToShow;
        public TextMeshProUGUI texts;
        public TextMeshProUGUI values;

        public void ShowStats()
        {
            texts.text = "";
            values.text = "";

            if (statsToShow == null) return;

            var playerStats = DictionaryHolder.Player.stats;
            for (int i = 0; i < statsToShow.Count; i++)
            {
                var statName = statsToShow[i].ToString();
                var statNemaAfter = Regex.Replace(statName, "([a-z])([A-Z])", "$1 $2");
                var statValue = playerStats.GetStat(statsToShow[i]);

                //var textValue = statNemaAfter + " " + statValue + "\n";
                texts.text += statNemaAfter + "\n";

                if (statValue > 0)
                    values.text += "<color=\"green\">" + statValue + "\n";
                else if(statValue <= 0)
                    values.text += "<color=\"red\">" + statValue + "\n";
                else
                    values.text += "<color=\"white\">" + statValue + "\n";
            }
        }
    }
}