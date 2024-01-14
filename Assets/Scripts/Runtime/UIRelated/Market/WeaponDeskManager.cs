using System;
using System.Collections.Generic;
using Data;
using Runtime.Managers;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Runtime.UIRelated.Market
{
    public class WeaponDeskManager : MonoBehaviour
    {
        public MarketUI marketUI;
        public List<WeaponSlot> weaponSlots;
        public ActiveCharacterSo activeCharacterSo;

        public void SetActiveCharacter()
        {
            activeCharacterSo = Resources.Load<ActiveCharacterSo>("CharacterData/ActiveCharacterData");
        }

        public void ActivateWeaponSlots()
        {
            weaponSlots.ForEach(t => t.gameObject.SetActive(false));
            for (int i = 0; i < activeCharacterSo.playerWeapons.Count; i++)
            {
                weaponSlots[i].gameObject.SetActive(true);
                weaponSlots[i]._weaponDatasSo = activeCharacterSo.playerWeapons[i].weaponDataSo;
                weaponSlots[i].weapon = activeCharacterSo.playerWeapons[i];
            }
        }

        public void AddEvents()
        {
            EventManager.Instance.WeaponsUpdatedEvent += ActivateWeaponSlots;
        }

        public void RemoveEvents()
        {
            EventManager.Instance.WeaponsUpdatedEvent -= ActivateWeaponSlots;
        }
    }
}