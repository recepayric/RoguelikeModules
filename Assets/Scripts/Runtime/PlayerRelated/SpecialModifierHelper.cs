using System;
using System.Collections.Generic;
using Data;
using Runtime.Enums;
using Runtime.Modifiers;
using UnityEngine;

namespace Runtime.PlayerRelated
{
    public class SpecialModifierHelper : MonoBehaviour
    {
        public Player playerScript;
        public List<SpecialModifiers> specialModifiersList;
        public List<Modifier> modifiersOnStart;
        public List<Modifier> modifiersOnGetHit;
        public List<Modifier> modifiersOnHealthChange;
        public List<Modifier> modifiersOnItemBuy;

        protected internal void UpdateModifiersOnStart()
        {
            for (int i = 0; i < modifiersOnHealthChange.Count; i++)
            {
                modifiersOnStart[i].ApplyEffect(playerScript);
            }
        }
        protected internal void UpdateModifiersOnHealthChange()
        {
            for (int i = 0; i < modifiersOnHealthChange.Count; i++)
            {
                modifiersOnHealthChange[i].ApplyEffect(playerScript);
            }
        }
        
        protected internal void UpdateModifiersOnGetHit()
        {
            for (int i = 0; i < modifiersOnGetHit.Count; i++)
            {
                modifiersOnGetHit[i].ApplyEffect(playerScript);
            }
        }
        
        
        public void SetSpecialModifiers(List<SpecialModifiers> pSpecialModifiersList)
        {
            for (int i = 0; i < pSpecialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(pSpecialModifiersList[i]);
                modifier.RegisterUser(gameObject);
                switch (modifier.useArea)
                {
                    case ModifierUseArea.OnStart:
                        if (!modifiersOnStart.Contains(modifier))
                            modifiersOnStart.Add(modifier);
                        break;
                    
                    case ModifierUseArea.OnHit:
                        break;
                    
                    case ModifierUseArea.OnGetHit:
                        if (!modifiersOnGetHit.Contains(modifier))
                            modifiersOnGetHit.Add(modifier);
                        break;
                    
                    case ModifierUseArea.OnBuyItem:
                        if (!modifiersOnItemBuy.Contains(modifier))
                            modifiersOnItemBuy.Add(modifier);
                        break;
                    
                    case ModifierUseArea.OnUpdate:
                        break;
                    
                    case ModifierUseArea.OnHealthChange:
                        if (!modifiersOnHealthChange.Contains(modifier))
                            modifiersOnHealthChange.Add(modifier);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        private void ClearSpecialModifierList(List<Modifier> modifiers)
        {
            for (int i = 0; i < modifiers.Count; i++)
            {
                modifiers[i].RemoveRegisteredUser(gameObject);
                modifiers[i].RemoveEffect(playerScript);
            }
            
            modifiers.Clear();
        }
        
        private void RemoveSpecialModifiers()
        {
            ClearSpecialModifierList(modifiersOnStart);
            ClearSpecialModifierList(modifiersOnGetHit);
            ClearSpecialModifierList(modifiersOnHealthChange);
            ClearSpecialModifierList(modifiersOnItemBuy);

            return;
            
            for (int i = 0; i < specialModifiersList.Count; i++)
            {
                var modifier = ModifierCreator.GetModifier(specialModifiersList[i]);
                modifier.RemoveRegisteredUser(gameObject);
                modifier.RemoveEffect(playerScript);
                switch (modifier.useArea)
                {
                    case ModifierUseArea.OnStart:
                        if (modifiersOnStart.Contains(modifier))
                            modifiersOnStart.Remove(modifier);
                        break;
                    
                    case ModifierUseArea.OnHit:
                        break;
                    
                    case ModifierUseArea.OnGetHit:
                        if (modifiersOnGetHit.Contains(modifier))
                            modifiersOnGetHit.Remove(modifier);
                        break;
                    
                    case ModifierUseArea.OnBuyItem:
                        if (modifiersOnItemBuy.Contains(modifier))
                            modifiersOnItemBuy.Remove(modifier);
                        break;
                    
                    case ModifierUseArea.OnUpdate:
                        break;
                    
                    case ModifierUseArea.OnHealthChange:
                        if (modifiersOnHealthChange.Contains(modifier))
                            modifiersOnHealthChange.Remove(modifier);
                        break;
                    
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                //modifiers.Add();
            }
        }
    }
}