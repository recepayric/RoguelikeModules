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

        public List<string> characterNames;
        public List<GameObject> charactersList;
        public GameObject lastActiveCharacter;

        public GameObject characterPosition;

        private void Awake()
        {
            AddEvents();
        }

        private void OnSetCharacterCameraController(bool status)
        {
            lastActiveCharacter.SetActive(status);
            cameraObject.SetActive(status);
        }


        private void OnCharacterSelect(string characterName)
        {
            var index = characterNames.IndexOf(characterName);

            if (lastActiveCharacter != null)
                lastActiveCharacter.SetActive(false);

            if (index >= charactersList.Count) return;

            lastActiveCharacter = charactersList[index];
            lastActiveCharacter.SetActive(true);
        }

        private void OnCharacterSelect(PoolKeys poolKeys)
        {
            if (lastActiveCharacter != null)
                BasicPool.instance.Return(lastActiveCharacter);

            var character = BasicPool.instance.Get(poolKeys);
            character.transform.position = characterPosition.transform.position;
            lastActiveCharacter = character;
        }

        private void AddEvents()
        {
            EventManager.Instance.CharacterSelectChangedEvent += OnCharacterSelect;
            EventManager.Instance.SetCharacterCameraStatusEvent += OnSetCharacterCameraController;
        }


        private void RemoveEvents()
        {
            EventManager.Instance.CharacterSelectChangedEvent -= OnCharacterSelect;
            EventManager.Instance.SetCharacterCameraStatusEvent -= OnSetCharacterCameraController;
        }

        private void OnDestroy()
        {
            RemoveEvents();
        }
    }
}