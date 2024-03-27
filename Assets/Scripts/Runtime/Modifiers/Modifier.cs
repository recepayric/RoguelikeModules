using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.PlayerRelated;
using Runtime.ProjectileRelated;
using Runtime.WeaponRelated;
using UnityEngine;

namespace Runtime.Modifiers
{
    [Serializable]
    public class Modifier
    {
        public bool isWeaponSkill;
        public SpecialModifiers specialModifier;
        public ModifierUseArea useArea;
        public Dictionary<GameObject, int> amountOfModifier;
        public  WeaponSkillDataSo weaponSkillDataSo;
        public WeaponSkillData skillData;

        public Modifier()
        {
            weaponSkillDataSo = ModifierCreator.weaponSkillDataSo;
        }

        public void GetSkillData()
        {
            skillData = weaponSkillDataSo.GetEffect(specialModifier);
        }

        public void SetSpecialModifier(SpecialModifiers pSpecialModifier)
        {
            specialModifier = pSpecialModifier;
        }
        
        public void SetWeaponSkill()
        {
            isWeaponSkill = true;
        }
        
        public void SetUseArea(ModifierUseArea modifierUseArea)
        {
            useArea = modifierUseArea;
        }

        public virtual void ApplyEffect()
        {
        }
        
        public virtual void ApplyEffect(Enemy enemy)
        {
        }

        public virtual void ApplyEffect(Player player)
        {
            //RegisterUser(player.gameObject);
        }

        public virtual void ApplyEffect(Projectile projectile)
        {
            //RegisterUser(projectile.gameObject);
        }

        public virtual void ApplyEffect(Weapon weapon)
        {
            //RegisterUser(weapon.gameObject);
        }

        public virtual void ApplyEffect(GameObject projectile, Projectile projectileScript, bool isCrit)
        {
            //RegisterUser(projectile);
        }

        public virtual void ApplyEffect(GameObject slashObject, Slash slash, bool isCrit)
        {
            
        }
        
        public virtual void ApplyEffect(GameObject swordObject, RotatingMeleeWeapons sword, bool isCrit)
        {
            
        }

        public virtual void RemoveEffect(Player player)
        {
            //RegisterUser(player.gameObject);
        }
        
        public virtual void RemoveEffect(Weapon weapon)
        {
            
        }

        public virtual void RegisterUser(GameObject gameObject)
        {
            if (amountOfModifier == null)
                amountOfModifier = new Dictionary<GameObject, int>();

            if (!amountOfModifier.ContainsKey(gameObject))
                amountOfModifier.Add(gameObject, 0);

            amountOfModifier[gameObject]++;
        }

        public virtual void RemoveRegisteredUser(GameObject gameObject)
        {
            if (amountOfModifier == null)
                amountOfModifier = new Dictionary<GameObject, int>();
            
            if (amountOfModifier.ContainsKey(gameObject))
                amountOfModifier.Remove(gameObject);
        }
        
    }
}