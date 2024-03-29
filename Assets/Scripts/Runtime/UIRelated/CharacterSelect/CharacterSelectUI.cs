using System.Collections.Generic;
using Data;
using Data.WeaponDataRelated;
using Runtime.Enums;
using Runtime.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.UIRelated.CharacterSelect
{
    public class CharacterSelectUI : MonoBehaviour, IOpenable
    {
        public Object[] _charactersObj;
        public Object[] _weaponObj;
        public CharacterDataSo[] characters;
        //public WeaponDataSo[] weapons;
        public List<WeaponDataSo> weapons;
        public CharacterDataSo selectedCharacter;
        public WeaponDataSo selectedWeapon;
        public GameObject charactersContainer;
        public GameObject weaponContainer;
        public bool isCreated = false;

        private void Start()
        {
            //OnOpened();
        }

        private void LoadAllCharacters()
        {
            _charactersObj = Resources.LoadAll("CharacterData", typeof(CharacterDataSo));
            characters = new CharacterDataSo[_charactersObj.Length];
            for (int i = 0; i < _charactersObj.Length; i++)
            {
                characters[i] = (CharacterDataSo)_charactersObj[i];
            }
        }

        private void LoadAllWeapons()
        {
            _weaponObj = Resources.LoadAll("WeaponDatas", typeof(WeaponDataSo));
            
            weapons = new List<WeaponDataSo>();
            
            for (int i = 0; i < _weaponObj.Length; i++)
            {
                var wep = (WeaponDataSo)_weaponObj[i];
                if(wep.isStarterWeapon)
                    weapons.Add(wep);
            }
        }

        private void CreateCharacterIcons()
        {
            if (isCreated) return;
            for (int i = 0; i < characters.Length; i++)
            {
                var icon = BasicPool.instance.Get(PoolKeys.CharacterIconUI);
                icon.transform.SetParent(charactersContainer.transform);
                icon.transform.localScale = Vector3.one;
                icon.transform.localPosition = Vector3.zero;
        
                var script = icon.GetComponent<CharacterFrameUI>();
                script.SetCharacterDetails(characters[i]);
                script.SetCharacterSelectUI(this);
            }
        }

        private void CreateWeaponIcons()
        {
            if (isCreated) return;
            for (int i = 0; i < weapons.Count; i++)
            {
                var icon = BasicPool.instance.Get(PoolKeys.WeaponIconUI);
                icon.transform.SetParent(weaponContainer.transform);
                icon.transform.localScale = Vector3.one;
                icon.transform.localPosition = Vector3.zero;
                var script = icon.GetComponent<WeaponFrameUI>();
                script.SetWeaponObject(weapons[i]);
                script.SetCharacterSelectUI(this);
            }
        }

        public void FinishCharacterSelect()
        {
            EventManager.Instance.CharacterSelected(selectedCharacter);
            EventManager.Instance.WeaponSelected(selectedWeapon);
            
            EventManager.Instance.OpenScreen(Screens.MapSelect, true);
        }


        #region SetCharacterDetails

        [Header("Details")] public TextMeshProUGUI levelText;
        public TextMeshProUGUI nameText;
        public TextMeshProUGUI descriptionText;
        public TextMeshProUGUI detailsText;
        public TextMeshProUGUI footerText;
        public Image headerImage;

        public void SetDetails(CharacterDataSo characterDataSo)
        {
            selectedCharacter = characterDataSo;
            levelText.text = characterDataSo.level.ToString();
            nameText.text = "•" + characterDataSo.characterName + "•";
            descriptionText.text = characterDataSo.description;
            footerText.text = "•" + characterDataSo.footer + "•";

            var stats = "";
            foreach (var stat in characterDataSo.BaseStats)
            {
                stats += "+" + stat.Value + " " + stat.Key.ToString() + "\n";
            }

            detailsText.text = stats;
            
            EventManager.Instance.CharacterSelectChanged(characterDataSo.mannqueenPoolKey);
        }

        #endregion

        #region SetWeaponDetails

        [Header("Details")] public TextMeshProUGUI weaponNameText;
        public TextMeshProUGUI weaponDescriptionText;
        public TextMeshProUGUI weaponDetailsText;
        public TextMeshProUGUI weaponFooterText;
        public Image weaponHeaderImage;

        public void SetWeaponDetails(WeaponDataSo weaponData)
        {
            selectedWeapon = weaponData;
            weaponNameText.text = "•" + weaponData.WeaponName + "•";
            //weaponDescriptionText.text = weaponData.Description;

            var stats = "";
            var attackDamage = "Attack Damage: " + weaponData.BaseDamage + "\n";
            var attackSpeed = "Attack Speed: " + weaponData.BaseAttackSpeed + "/s\n";
            var attackRange = "Range: " + weaponData.BaseAttackRange + "\n";
            var critChance = "crit: " + weaponData.BaseAttackRange + "%\n";
            stats = attackDamage + attackSpeed + attackRange + critChance;
            weaponDetailsText.text = stats;
            
            EventManager.Instance.WeaponSelectChanged(weaponData.DummyWeaponKey);
        }

        #endregion

        public void OnOpened()
        {
            
            LoadAllCharacters();
            LoadAllWeapons();
            CreateCharacterIcons();
            CreateWeaponIcons();

            if (selectedCharacter == null)
            {
                selectedCharacter = characters[0];
                selectedWeapon = weapons[0];
            }
            //set initial selected character!
            SetDetails(selectedCharacter);
            SetWeaponDetails(selectedWeapon);
            EventManager.Instance.SetCharacterCameraStatus(true);

            isCreated = true;
        }

        public void OnClosed()
        {
            EventManager.Instance.SetCharacterCameraStatus(false);
        }
    }
}