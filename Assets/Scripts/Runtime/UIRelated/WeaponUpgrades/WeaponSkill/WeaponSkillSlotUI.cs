using Data;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Runtime.UIRelated.WeaponUpgrades.WeaponSkill
{
    public class WeaponSkillSlotUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler
    {
        public bool isSlotFull = false;
        public WeaponUpgradeSkillUI equippedSkill;
        public WeaponUpgradeScreenUI WeaponUpgradeScreenUI;
        public GameObject skillPrefab;
        public bool isBought;
        public bool canBuy;
        public Button btnBuy;

        public void SetStatus(bool isBought)
        {
            this.isBought = isBought;
        }
        
        public void SetAvailability(bool canBeBought)
        {
            canBuy = canBeBought;
            btnBuy.interactable = canBeBought;
        }
        
        public void EquipSkill(WeaponSkillData skillData, int tier)
        {
            equippedSkill.gameObject.SetActive(true);
            equippedSkill.SetSkill(skillData, tier);
            isSlotFull = true;
        }

        public void UnEquipSkill()
        {
            isSlotFull = false;
            equippedSkill.gameObject.SetActive(false);
        }
        
        public void OpenSkillSelectPanel()
        {
            if(isBought)
                WeaponUpgradeScreenUI.OpenSkillList(this);
            else
            {
                isBought = true;
                WeaponUpgradeScreenUI.UpdateStatus();
            }
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
        }
    }
}