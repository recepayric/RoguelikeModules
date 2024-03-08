using System;
using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Managers;
using UnityEngine;

namespace Runtime.UIRelated.RawCameraRelated
{
    public class CharacterSelectCameraController : MonoBehaviour
    {
        public GameObject cameraObject;
        public GameObject lastActiveCharacter;
        public GameObject characterPosition;
        public CharacterMannqueen characterMannqueen;
        public bool hasWeaponSelected = false;
        public PoolKeys selectedWeapon;

        private void Awake()
        {
            AddEvents();
        }

        private void OnSetCharacterCameraController(bool status)
        {
            lastActiveCharacter.SetActive(status);
            cameraObject.SetActive(status);
        }

        private void OnCharacterSelect(PoolKeys poolKeys)
        {
            if (lastActiveCharacter != null)
                BasicPool.instance.Return(lastActiveCharacter);

            var character = BasicPool.instance.Get(poolKeys);
            character.transform.position = characterPosition.transform.position;
            lastActiveCharacter = character;
            characterMannqueen = lastActiveCharacter.GetComponent<CharacterMannqueen>();

            if (hasWeaponSelected)
            {
                characterMannqueen.SetWeapon(selectedWeapon);
            }
        }
        
        public void OnWeaponSelectChanged(PoolKeys poolKeys)
        {
            hasWeaponSelected = true;
            selectedWeapon = poolKeys;
            characterMannqueen.SetWeapon(selectedWeapon);
        }

        private void AddEvents()
        {
            EventManager.Instance.CharacterSelectChangedEvent += OnCharacterSelect;
            EventManager.Instance.SetCharacterCameraStatusEvent += OnSetCharacterCameraController;
            EventManager.Instance.WeaponSelectChangedEvent += OnWeaponSelectChanged;
        }


        private void RemoveEvents()
        {
            EventManager.Instance.CharacterSelectChangedEvent -= OnCharacterSelect;
            EventManager.Instance.SetCharacterCameraStatusEvent -= OnSetCharacterCameraController;
            EventManager.Instance.WeaponSelectChangedEvent -= OnWeaponSelectChanged;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }
    }
}