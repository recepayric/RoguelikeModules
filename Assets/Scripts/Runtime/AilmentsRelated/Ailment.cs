using System;
using System.Collections.Generic;
using Runtime.Enums;
using UnityEngine;
using UnityEngine.Events;

namespace Runtime.AilmentsRelated
{
    [Serializable]
    public class Ailment
    {
        public AilmentTypes AilmentType;
        public AllStats stackStatForAilment;

        public bool isActive;
        public bool canBeStakced;
        private float timerForAilment;
        private float toleranceTimer = 0.1f;
        private float timeToApplyAilment = 1;
        private float lastEffectAmount;
        public List<int> finishedAilments;
        public List<float> ailmentRemainingTimes;
        public List<float> ailmentEffects;
        private UnityAction<float> _applyEffectEvent;
        private UnityAction _ailmentFinished;
        private UnityAction _ailmentAdded;
        private UnityAction<float> _effectChange;

        public Ailment(AilmentTypes ailmentType, AllStats statForMaxStack)
        {
            AilmentType = ailmentType;
            stackStatForAilment = statForMaxStack;
            ailmentRemainingTimes = new List<float>();
            ailmentEffects = new List<float>();
            finishedAilments = new List<int>();
        }

        public void UpdateAilment()
        {
            if (ailmentRemainingTimes.Count == 0)
            {
                timerForAilment = 0;
                return;
            }

            ReduceTimes();
            RemoveFinishedAilments();
            
            if(!isActive)
                EndAilments();
            
            timerForAilment += Time.deltaTime;

            if (timerForAilment >= timeToApplyAilment)
            {
                timerForAilment = 0;
                ApplyAilmentEffect();
            }
        }

        private void ReduceTimes()
        {
            bool isAllFinished = true;
            for (int i = 0; i < ailmentRemainingTimes.Count; i++)
            {
                ailmentRemainingTimes[i] -= Time.deltaTime;
                if (ailmentRemainingTimes[i] <= toleranceTimer)
                {
                    finishedAilments.Add(i);
                }
            }
        }

        private void RemoveFinishedAilments()
        {
            for (int i = 0; i < finishedAilments.Count; i++)
            {
                ailmentRemainingTimes.RemoveAt(finishedAilments[i]);
                ailmentEffects.RemoveAt(finishedAilments[i]);
            }
            CalculateEffect();
            finishedAilments.Clear();

            if (ailmentRemainingTimes.Count == 0)
                isActive = false;
        }

        private void EndAilments()
        {
            _ailmentFinished?.Invoke();
        }

        private void CalculateEffect()
        {
            var effectTotal = 0f;
            for (int i = 0; i < ailmentRemainingTimes.Count; i++)
            {
                effectTotal += ailmentEffects[i];
            }
            
            if (effectTotal != 0 && lastEffectAmount != effectTotal)
            {
                lastEffectAmount = effectTotal;
                _effectChange?.Invoke(effectTotal);
            }
        }

        private void ApplyAilmentEffect()
        {
            _applyEffectEvent?.Invoke(lastEffectAmount);
        }

        public void AddAilmentEffect(float time, float effect)
        {
            if (!CheckCanBeStacked())
            {
                ailmentEffects[0] = effect;
                ailmentRemainingTimes[0] = time;
            }
            else
            {
                ailmentEffects.Add(effect);
                ailmentRemainingTimes.Add(time);
            }

            isActive = true;
            CalculateEffect();
        }

        public void OnApplyEffect(UnityAction<float> action)
        {
            _applyEffectEvent = action;
        }

        public void OnFinishEffect(UnityAction action)
        {
            _ailmentFinished = action;
        }

        public void OnEffectChange(UnityAction<float> action)
        {
            _effectChange = action;
        }

        public void EndAilment()
        {
            isActive = false;
            ClearAilment();
            _ailmentFinished?.Invoke();
        }

        public void ClearAilment()
        {
            ailmentRemainingTimes.Clear();
            ailmentEffects.Clear();
        }

        private bool CheckCanBeStacked()
        {
            var maxStackNum = DictionaryHolder.Player.stats.GetStat(stackStatForAilment) + 1;
            var currentStackNum = ailmentEffects.Count;
            return currentStackNum < maxStackNum;
        }
    }
}