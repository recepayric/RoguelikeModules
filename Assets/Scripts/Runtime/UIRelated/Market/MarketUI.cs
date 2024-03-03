using System.Collections.Generic;
using Data;
using Data.WeaponDataRelated;
using Runtime.Enums;
using Runtime.ItemsRelated;
using Runtime.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Runtime.UIRelated.Market
{
    public class MarketUI : MonoBehaviour, IOpenable
    {
        public Object[] _weaponObj;
        public WeaponDataSo[] weapons;

        public WeaponDeskManager weaponDeskManager;
        public ItemDataSo itemSo;
        public TextMeshProUGUI statsText;
        public Player playerScript;
        public Stats playerStats;

        public List<MarketItemSlots> itemSlots;
        public List<AllStats> statsToShow;

        [Header("Item Details")] public GameObject itemDetails;
        public TextMeshProUGUI itemHeader;
        public TextMeshProUGUI itemStats;
        public TextMeshProUGUI itemFooter;

        public float weaponChance = 0;

        private void LoadAllWeapons()
        {
            _weaponObj = Resources.LoadAll("WeaponDatas", typeof(WeaponDataSo));

            weapons = new WeaponDataSo[_weaponObj.Length];
            for (int i = 0; i < _weaponObj.Length; i++)
            {
                weapons[i] = (WeaponDataSo)_weaponObj[i];
            }
        }

        public void NextFloor()
        {
            EventManager.Instance.LoadTower();
        }

        public void BuyItem(Item item)
        {
            SetPlayerScript();
            SetStats();
        }
        
        public void BuyWeapon()
        {
            SetPlayerScript();
            SetStats();
        }

        public void ResetMarket()
        {
            GetRandomItems();
        }

        public void GetRandomItems()
        {
            var totalItemCount = itemSo.itemData2.Count;

            for (int i = 0; i < itemSlots.Count; i++)
            {
                var isWeapon = Random.Range(0, 1f) <= weaponChance;

                if (isWeapon)
                {
                    var randItem = weapons[Random.Range(0, weapons.Length)];
                    itemSlots[i].SetItem(randItem);
                }
                else
                {
                    var randItem = itemSo.itemData2[Random.Range(0, totalItemCount)];
                    itemSlots[i].SetItem(randItem);
                }
            }
        }

        public void SetItemDetails(Item item)
        {
            itemDetails.SetActive(true);
            if (item == null)
            {
                itemHeader.text = "null";
                return;
            }

            itemHeader.text = item.name;
            itemFooter.text = item.description;

            itemStats.text = "";
            for (int i = 0; i < item.statNames.Count; i++)
            {
                var statValue = item.statValues[i];
                var statText = "";
                if (statValue > 0)
                    statText = "<color=\"green\">+" + statValue + "</color>";
                else
                    statText = "<color=\"red\">" + statValue + "</color>";
                itemStats.text += statText + " " + item.statNames[i] + "\n";
            }
        }

        public void StopShowingItemDetails()
        {
            itemDetails.SetActive(false);
        }

        public void SetWeaponDetails(WeaponDataSo weaponDatasSo)
        {
            if (weaponDatasSo == null)
            {
                itemDetails.SetActive(false);
                return;
            }

            itemDetails.SetActive(true);
        }


        public void SetPlayerScript()
        {
            if (DictionaryHolder.Player == null) return;
            playerScript = DictionaryHolder.Player;
            playerStats = playerScript.stats;
        }

        public void SetStats()
        {
            if (playerScript == null) return;

            statsText.text = "";
            for (int i = 0; i < statsToShow.Count; i++)
            {
                var statValue = playerStats.GetStat(statsToShow[i]);
                var stat = statsToShow[i] + ": ";
                var statValueText = "";
                if (statValue > 0)
                    statValueText = "<color=\"green\">" + statValue + "</color>\n";
                else
                    statValueText = "<color=\"red\">" + statValue + "</color>\n";


                statsText.text += stat + statValueText;
            }
        }

        [Button]
        private void SetWeapons()
        {
            weaponDeskManager.SetActiveCharacter();
            weaponDeskManager.ActivateWeaponSlots();
        }

        public void OnOpened()
        {
            LoadAllWeapons();
            SetPlayerScript();
            SetStats();
            GetRandomItems();
            SetWeapons();

            weaponDeskManager.AddEvents();
        }

        public void OnClosed()
        {
            weaponDeskManager.RemoveEvents();
        }
    }
}