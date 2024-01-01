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
        private Object[] _charactersObj;
        private Object[] _weaponObj;
        public CharacterDataSo[] characters;
        public WeaponDataSo[] weapons;
        public CharacterDataSo selectedCharacter;
        public WeaponDataSo selectedWeapon;
        public GameObject charactersContainer;
        public GameObject weaponContainer;

        private void Start()
        {
            OnOpened();
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

            weapons = new WeaponDataSo[_weaponObj.Length];
            for (int i = 0; i < _weaponObj.Length; i++)
            {
                weapons[i] = (WeaponDataSo)_weaponObj[i];
            }
        }

        private void CreateCharacterIcons()
        {
            for (int i = 0; i < characters.Length; i++)
            {
                var icon = BasicPool.instance.Get(PoolKeys.CharacterIconUI);
                icon.transform.SetParent(charactersContainer.transform);

                var script = icon.GetComponent<CharacterFrameUI>();
                script.SetCharacterDetails(characters[i]);
                script.SetCharacterSelectUI(this);
            }
        }

        private void CreateWeaponIcons()
        {
            for (int i = 0; i < weapons.Length; i++)
            {
                var icon = BasicPool.instance.Get(PoolKeys.WeaponIconUI);
                icon.transform.SetParent(weaponContainer.transform);

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
            
            EventManager.Instance.CharacterSelectChanged(characterDataSo.characterName);
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
        }

        #endregion

        public void OnOpened()
        {
            LoadAllCharacters();
            LoadAllWeapons();
            CreateCharacterIcons();
            CreateWeaponIcons();

            //set initial selected character!
            selectedCharacter = characters[0];
            SetDetails(selectedCharacter);
            EventManager.Instance.SetCharacterCameraStatus(true);
        }

        public void OnClosed()
        {
            EventManager.Instance.SetCharacterCameraStatus(false);
        }
    }
}