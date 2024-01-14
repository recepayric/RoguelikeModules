using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Interfaces;
using Runtime.Modifiers;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Runtime.WeaponRelated
{
    public class RotatingMeleeWeapons : MonoBehaviour, IPoolObject
    {
        #region Damage Related Variables

        private IDamageable _ignoredEnemy;
        private IShooter _shooter;
        private float _damage;
        private List<Modifier> _modifiers;

        #endregion

        public void HitTarget(IDamageable enemy)
        {
            if (enemy == _ignoredEnemy)
                return;

            _damage = _shooter?.GetDamage() ?? 0;
            //ResetTravelData();
            var isCrit = Random.Range(0, 1f) <= _shooter?.GetCriticalDamageChance();

            enemy.DealDamage((int)_damage, isCrit);

            //Apply Ailments!
            //todo redo this!!!
            //
            // if (weapon.weaponStats.addBurn)
            //     enemy.AddBurning(weapon.weaponStats.burnTime, weapon.weaponStats.burnDamage);
            //
            // if (weapon.weaponStats.addFreeze)
            //     enemy.AddFreeze(weapon.weaponStats.freezeTime, weapon.weaponStats.freezeEffect);
            //
            // if (weapon.weaponStats.addShock)
            //     enemy.AddShock(weapon.weaponStats.shockTime, weapon.weaponStats.shockEffect);

            //todo add modifiers!!!
            if (_modifiers != null)
            {
                foreach (var modifier in _modifiers)
                {
                    modifier.ApplyEffect(gameObject, this, isCrit);
                }
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            //var damageable = DictionaryHolder.Damageables[other.gameObject];
            var damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                HitTarget(damageable);
            }
        }
        
        public void SetModifiers(List<Modifier> modifiers)
        {
            _modifiers = modifiers;
        }

        public void SetShooter(IShooter shooter)
        {
            _shooter = shooter;
        }


        public PoolKeys PoolKeys { get; set; }
        public void OnGet()
        {
            DictionaryHolder.RotatingMeleeWeapons.Add(gameObject, this);
        }
        public void OnReturn()
        {
            DictionaryHolder.RotatingMeleeWeapons.Remove(gameObject);
        }

    }
}