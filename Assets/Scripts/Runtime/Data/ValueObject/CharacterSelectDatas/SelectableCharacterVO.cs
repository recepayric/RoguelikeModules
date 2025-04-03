using System;
using Runtime.Data.UnityObject;
using Runtime.Enums;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Runtime.Data.ValueObject.CharacterSelectDatas
{
    [Serializable]
    public class SelectableCharacterVO
    {
        public event UnityAction SelectCharacterEvent;
        public PoolKey characterPoolKey;
        public RD_Player playerData;

        public void SelectCharacter()
        {
            SelectCharacterEvent?.Invoke();
        }
    }
}