using Runtime.ItemsRelated;
using TMPro;
using UnityEngine;

namespace Runtime.UIRelated.GameOverRelated
{
    public class GameEndItemDetailsUI : MonoBehaviour
    {
        public TextMeshProUGUI itemHeader;
        public TextMeshProUGUI itemDetails;
        public TextMeshProUGUI itemFooter;

        public void ShowWeaponDetails(Weapon weapon)
        {
            itemHeader.text = weapon.weaponDataSo.WeaponName;

            itemDetails.text = "Damage: " + weapon.GetDamage();
            itemDetails.text += "\nAttack Speed: " + weapon.weaponStats.attackSpeed + "s";
            itemDetails.text += "\nAttack Range: " + weapon.weaponStats.range;
            
            //todo special modifier if it has any!
        }

        public void ShowItemDetails(Item item)
        {
            itemHeader.text = item.name;
        }
    }
}