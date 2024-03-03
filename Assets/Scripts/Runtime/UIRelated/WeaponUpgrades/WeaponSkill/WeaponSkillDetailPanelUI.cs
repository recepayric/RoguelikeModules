using Data;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated.WeaponUpgrades.WeaponSkill
{
    public class WeaponSkillDetailPanelUI : MonoBehaviour
    {
        public TextMeshProUGUI skillNameText;
        public TextMeshProUGUI skillDetailsText;
        public TextMeshProUGUI skillFooterText;

        public void SetDetails(WeaponSkillData skillData)
        {
            skillNameText.text = skillData.modifierName.ToString();
        }
    }
}