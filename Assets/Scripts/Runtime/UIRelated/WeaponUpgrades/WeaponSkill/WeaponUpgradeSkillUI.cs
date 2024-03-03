using Data;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.WeaponUpgrades.WeaponSkill
{
    public class WeaponUpgradeSkillUI : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler , IPointerClickHandler
    {
        public WeaponUpgradeScreenUI weaponUpgradeScreenUI;
        public WeaponSkillSlotUI weaponSkillSlotUI;
        public WeaponSkillData skillData;
        public TextMeshProUGUI skillImageText;
        public int tier;
        public bool isEquipped;

        public void EquipSkill()
        {
            Debug.Log("Equipping skill " + skillData.modifierName);
            //skillData.isEquipped = true;
            weaponUpgradeScreenUI.EquipSkill(weaponSkillSlotUI, this);
        }
        
        public void UnequipSkill()
        {
            
            weaponUpgradeScreenUI.UnEquipSkill(weaponSkillSlotUI, this);
        }
        
        public void SetSkill(WeaponSkillData pSkillData, int pTier)
        {
            tier = pTier;
            skillData = pSkillData;
            skillImageText.text = pSkillData.skillNumber + "-" + tier;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Over the skill!");
            weaponUpgradeScreenUI.OpenSkillDetail(skillData);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            weaponUpgradeScreenUI.CloseSkillDetail();

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!isEquipped && eventData.button == PointerEventData.InputButton.Right)
                EquipSkill();
            else if(isEquipped && eventData.button == PointerEventData.InputButton.Right)
            {
                UnequipSkill();
            }
        }
    }
}