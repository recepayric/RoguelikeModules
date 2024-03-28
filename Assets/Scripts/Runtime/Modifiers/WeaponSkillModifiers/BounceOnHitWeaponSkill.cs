using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Modifiers.WeaponSkillModifiers
{
    public class BounceOnHitWeaponSkill : Modifier
    {
        public int[] BounceChances = new[] { 3, 3, 3, 3 };
        public int[] BounceAmount = new[] { 1, 2, 3, 4 };

        public int tier = 0;
        public Dictionary<GameObject, int> projectiles = new Dictionary<GameObject, int>();

        public Dictionary<GameObject, int> addedAmount = new Dictionary<GameObject, int>();
        
        public BounceOnHitWeaponSkill()
        {
            SetSpecialModifier(SpecialModifiers.BounceOnHitSkill);
            SetWeaponSkill();
            SetUseArea(ModifierUseArea.OnStart);
            GetSkillData();
        }

        public override void ApplyEffect(Weapon weapon)
        {
            base.ApplyEffect(weapon);

            //var addAmount = 3;
            int tier = skillData.slotData.Find(t => t.weapon == weapon.gameObject).tier;

            var bounceAmount = (int) skillData.effectPerTier[tier];
            
            if (addedAmount.ContainsKey(weapon.gameObject))
            {
                weapon.weaponStats.AddStatFromTree(AllStats.BounceNumber, -addedAmount[weapon.gameObject]);;
            }
            else
            {
                addedAmount.Add(weapon.gameObject, bounceAmount);
            }

            //Debug.Log("Bounce before adding modifier: " + weapon.weaponStats.bounceNum);
            Debug.Log("Bounce before adding modifier: " + weapon.weaponStats.GetStat(AllStats.BounceNumber));
            weapon.weaponStats.AddStatFromTree(AllStats.BounceNumber, bounceAmount);;
            //Debug.Log("Bounce after adding modifier: " + weapon.weaponStats.bounceNum);
            Debug.Log("Bounce after adding modifier: " + weapon.weaponStats.GetStat(AllStats.BounceNumber));
            
        }

        public override void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            base.ApplyEffect(projectile, projectileScript, isCrit);
        }

        public override void ApplyEffect()
        {
            base.ApplyEffect();
        }
    }
}