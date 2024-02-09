using System;
using System.Collections.Generic;
using Runtime.Configs;
using Runtime.Enums;
using Runtime.Interfaces;
using UnityEngine;

namespace Runtime.AilmentsRelated
{
    [Serializable]
    public class Ailments
    {
        public GameObject gameObject;
        public IDamageable damageable;
        public List<GameObject> enemiesToSpreadAilment;
        
        //Ailments
        [Header("Ailment Objects")] public GameObject burnAilmentObject;
        public GameObject freezeAilmentObject;
        public GameObject shockAilmentObject;
        public GameObject bleedingAilmentObject;
        
        //Ailment Times
        [Header("Ailment Times")] public float burnTime;
        public float freezeTime;
        public float shockTime;
        
        [Header("Ailments")] public bool isBurning;
        public bool isFrozen;
        public bool isShocked;
        public bool isBleeding;
        public bool isStunned;
        
        //Ailment Effects
        [Header("Ailment Effects")]
        public float freezeEffect;
        public float shockEffect;

        private float burningTimer;

        private bool isInitialised = false;
        public Ailment burnAilment;
        public Ailment freezeAilment;
        public Ailment shockAilment;
        public Ailment bleedAilment;
        public Ailment stunAilment;

        private Dictionary<AilmentTypes, Ailment> _allAilment;

        public void Initialise(bool forceInitialise = false)
        {
            if (isInitialised && !forceInitialise)
                return;
            
            _allAilment = new Dictionary<AilmentTypes, Ailment>();
            burnAilment = new Ailment(AilmentTypes.Burn, AllStats.MaxBurnStack);
            freezeAilment = new Ailment(AilmentTypes.Freeze, AllStats.MaxFreezeStack);
            shockAilment = new Ailment(AilmentTypes.Shock, AllStats.MaxShockStack);
            bleedAilment = new Ailment(AilmentTypes.Bleed, AllStats.MaxBleedStack);
            stunAilment = new Ailment(AilmentTypes.Stun, AllStats.MaxStunStack);
            
            _allAilment.Add(AilmentTypes.Burn, burnAilment);
            _allAilment.Add(AilmentTypes.Freeze, freezeAilment);
            _allAilment.Add(AilmentTypes.Shock, shockAilment);
            _allAilment.Add(AilmentTypes.Bleed, bleedAilment);
            _allAilment.Add(AilmentTypes.Stun, stunAilment);
            
            //Apply Effects!!!
            burnAilment.OnApplyEffect(ApplyBurn);
            bleedAilment.OnApplyEffect(ApplyBleed);
            
            //Effect Change - Freeze, Stun, Shock
            freezeAilment.OnEffectChange(FreezeEffectChange);
            shockAilment.OnEffectChange(ShockEffectChange);
            
            //Effect End - All
            stunAilment.OnFinishEffect(FinishStun);
            bleedAilment.OnFinishEffect(FinishBleeding);
            burnAilment.OnFinishEffect(FinishBurn);
            freezeAilment.OnFinishEffect(FinishFreeze);
            shockAilment.OnFinishEffect(FinishShock);

            isInitialised = true;
        }

        public void FinishAllAilments()
        {
            foreach (var ail in _allAilment)
            {
                ail.Value.EndAilment();
            }
        }
        
        public void UpdateAilments()
        {
            stunAilment.UpdateAilment();
            bleedAilment.UpdateAilment();
            burnAilment.UpdateAilment();
            freezeAilment.UpdateAilment();
            shockAilment.UpdateAilment();
            
        }

        #region Stun

        private void AddStun(float time, float effect)
        {
            isStunned = true;
            stunAilment.AddAilmentEffect(time, effect);
        }
        
        private void FinishStun()
        {
            isStunned = false;
        }

        #endregion
        
        #region Bleed

        private void FinishBleeding()
        {
            isBleeding = false;
            //bleedingAilmentObject.SetActive(true);
        }

        public void AddBleeding(float time, float effect)
        {
            bleedAilment.AddAilmentEffect(time, effect);
            isBleeding = true;
        }
        
        
        private void ApplyBleed(float damage)
        {
            damageable.DealDamage((int)damage, false);
        }
        
        #endregion
        
        #region Burn

        private void AddBurning(float burnTimeToAdd, float burningDamage)
        {
            isBurning = true;
            burnTime = burnTimeToAdd;
            burnAilmentObject.SetActive(true);
            burnAilment.AddAilmentEffect(burnTime, burningDamage);
        }

        private void FinishBurn()
        {
            isBurning = false;
            burnTime = 0;
            burningTimer = 0;
            burnAilmentObject.SetActive(false);
        }
        
        //Damage related!!
        private void ApplyBurn(float damage)
        {
            damageable.DealDamage((int)damage, false);
        }

        #endregion

        #region Freeze

        private void AddFreeze(float freezeTimeToAdd, float pFreezeEffect)
        {
            isFrozen = true;
            freezeTime = freezeTimeToAdd;
            freezeEffect = pFreezeEffect;
            freezeAilmentObject.SetActive(true);
        }

        private void FinishFreeze()
        {
            isFrozen = false;
            freezeTime = 0;
            freezeEffect = 0;
            freezeAilmentObject.SetActive(false);
        }

        private void FreezeEffectChange(float effect)
        {
            freezeEffect = effect;
        }

        #endregion

        #region Shock

        private void AddShock(float shockTimeToAdd, float shockEffect)
        {
            isShocked = true;
            shockTime = shockTimeToAdd;
            this.shockEffect = shockEffect;
            shockAilmentObject.SetActive(true);
        }

        private void FinishShock()
        {
            isShocked = true;
            shockTime = 0;
            shockEffect = 0;
            shockAilmentObject.SetActive(false);
        }
        
        private void ShockEffectChange(float effect)
        {
            shockEffect = effect;
        }

        #endregion
        
        public void AddElementalAilment(ElementModifiers element, float time, float effect, int spreadAmount)
        {
            if (element == ElementModifiers.Fire)
            {
                AddBurning(time, effect);
            }else if (element == ElementModifiers.Ice)
            {
                AddFreeze(time, effect);
            }else if (element == ElementModifiers.Lightning)
            {
                AddShock(time, effect);
            }else if (element == ElementModifiers.Bleed)
            {
                AddBleeding(time, effect);
            }else if (element == ElementModifiers.Stun)
            {
                AddStun(time, effect);
            }


            if (enemiesToSpreadAilment == null)
                enemiesToSpreadAilment = new List<GameObject>();
            
            enemiesToSpreadAilment.Clear();
            
            for (int i = 0; i < spreadAmount; i++)
            {
                var closeEnemy = GetClosestEnemy(enemiesToSpreadAilment);
                Debug.Log("Checking for closest enemies!!!:: " + closeEnemy);
                if(closeEnemy == gameObject)
                    break;
                
                Debug.Log("Fire is spreading!!!!!!");
                enemiesToSpreadAilment.Add(closeEnemy);
                DictionaryHolder.Damageables[closeEnemy].AddElementalAilment(element, time, effect, 0);
            }
        }
        
        private GameObject GetClosestEnemy(List<GameObject> ignoreList)
        {
            //todo do this in one loop!!!
            var closestEnemy = gameObject;
            var closestDistance = 99999f;
            bool isInIgnoreList = false;
            foreach (var enemy in DictionaryHolder.Enemies)
            {
                //Check for this enemy
                if (enemy.Key == gameObject)
                    continue;
                
                //Check for ignore list
                for (int i = 0; i < ignoreList.Count; i++)
                {
                    if (enemy.Key == ignoreList[i])
                    {
                        isInIgnoreList = true;
                        break;
                    }
                }
                
                if(isInIgnoreList)
                    continue;

                var distance = Vector3.Distance(gameObject.transform.position, enemy.Key.transform.position);
                if (distance <= GameConfig.FireSpreadDistance && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy.Key;
                }
            }

            return closestEnemy;
        }
    }
}