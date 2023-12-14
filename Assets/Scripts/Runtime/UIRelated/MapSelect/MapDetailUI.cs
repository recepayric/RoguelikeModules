using Runtime.Managers;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated.MapSelect
{
    public class MapDetailUI : MonoBehaviour
    {
        public int currentTier;
        public TextMeshProUGUI towerNameText;
        public TextMeshProUGUI towerModifiers;
        public TextMeshProUGUI towerTierText;
        public TextMeshProUGUI towerRarityText;
        public TextMeshProUGUI towerDropRateText;
        public TextMeshProUGUI towerExperienceRateText;

        public void UpdateTowerDetails()
        {
            EventManager.Instance.PrepareTower(currentTier);
            var tower = ScriptDictionaryHolder.CurrentTower;
            towerNameText.text = tower.towerName;

            towerModifiers.text = "";

            for (int i = 0; i < tower.TowerModifiers.Count; i++)
            {
                towerModifiers.text += "<color=\"red\">" + tower.TowerModifiers[i].TextToShow + "\n";
            }

            towerTierText.text = "Tier: <color=\"green\">" + tower.tier;
            towerRarityText.text = "Rarity: <color=\"yellow\">Rare" ;
            towerDropRateText.text = "Drop Rate: <color=\"green\">" + tower.DropRateIncrease + "%";
            towerExperienceRateText.text = "Experience Increase: <color=\"green\">" + tower.ExperienceRateIncrease + "%";
        }

        public void SetTier(int pTier)
        {
            currentTier = pTier;
        }

        public void RerollMap()
        {
            EventManager.Instance.CreateTower(currentTier);
            EventManager.Instance.PrepareTower(currentTier);
            UpdateTowerDetails();
        }

        public void StartTower()
        {
            EventManager.Instance.LoadTower();
        }
    }
}