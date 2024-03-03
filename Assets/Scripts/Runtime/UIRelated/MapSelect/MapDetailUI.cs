using System;
using Data;
using Runtime.Enums;
using Runtime.Managers;
using Runtime.TowerRelated;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UIRelated.MapSelect
{
    public class MapDetailUI : MonoBehaviour
    {
        private CurrencyDataSo _currencyDataSo;
        public Tower selectedTower;
        public int currentTier;
        public TextMeshProUGUI towerNameText;
        public TextMeshProUGUI towerModifiers;
        public TextMeshProUGUI towerTierText;
        public TextMeshProUGUI towerRarityText;
        public TextMeshProUGUI towerDropRateText;
        public TextMeshProUGUI towerExperienceRateText;
        public TextMeshProUGUI txtRerollText;
        public TextMeshProUGUI txtUpgradeText;

        public Button btnReroll;
        public Button btnUpgrade;
        

        public void UpdateTowerDetails()
        {
            Debug.Log(gameObject.name);
            var tower = selectedTower;
            towerNameText.text = tower.towerName;

            towerModifiers.text = "";

            for (int i = 0; i < tower.TowerModifiers.Count; i++)
            {
                towerModifiers.text += "<color=\"red\">" + tower.TowerModifiers[i].TextToShow + "\n";
            }

            towerTierText.text = "Tier: <color=\"green\">" + (tower.tier+1);
            towerRarityText.text = "Rarity: <color=\"yellow\">" + (tower.towerRarity) ;
            towerDropRateText.text = "Drop Rate: <color=\"green\">" + tower.DropRateIncrease + "%";
            towerExperienceRateText.text = "Experience Increase: <color=\"green\">" + tower.ExperienceRateIncrease + "%";

            btnReroll.interactable = CanReroll();
            btnUpgrade.interactable = CanUpgrade();

            txtRerollText.text = "Reroll("+tower.getRerollCost()+")";
            txtUpgradeText.text = "Upgrade("+tower.GetUpgradeCost()+")";
            
            if(tower.tier < 0)
                btnUpgrade.gameObject.SetActive(false);
            else
                btnUpgrade.gameObject.SetActive(true);
        }

        private bool CanUpgrade()
        {
            var currencyAmount = _currencyDataSo.GetCollectableAmount(CollectableTypes.TowerFragment);
            return currencyAmount >= selectedTower.GetUpgradeCost();
        }

        private bool CanReroll()
        {
            var currencyAmount = _currencyDataSo.GetCollectableAmount(CollectableTypes.TowerFragment);
            return currencyAmount >= selectedTower.getRerollCost();
        }

        public void SetTier(int pTier)
        {
            currentTier = pTier;
            Debug.Log("Preparing tower with tier: " + pTier);
            EventManager.Instance.PrepareTower(currentTier);
        }

        public void RerollMap()
        {
            EventManager.Instance.CreateTower(currentTier);
            EventManager.Instance.PrepareTower(currentTier);
            _currencyDataSo.AddCollectable(CollectableTypes.TowerFragment, -selectedTower.getRerollCost());
            //UpdateTowerDetails();
        }

        public void UpgradeMap()
        {
            EventManager.Instance.UpgradeTower();
            EventManager.Instance.PrepareTower(currentTier);
            _currencyDataSo.AddCollectable(CollectableTypes.TowerFragment, -selectedTower.GetUpgradeCost());
        }

        public void StartTower()
        {
            EventManager.Instance.LoadTower();
        }

        public void OnUpdateTower(Tower tower)
        {
            selectedTower = tower;
            UpdateTowerDetails();
        }
        

        private void Awake()
        {
            Debug.Log("Registered!!!!!");
            _currencyDataSo = Resources.Load<CurrencyDataSo>("CollectableData");
            EventManager.Instance.UpdateTowerEvent += OnUpdateTower;
        }

        private void OnDestroy()
        {
            EventManager.Instance.UpdateTowerEvent -= OnUpdateTower;
        }
    }
}