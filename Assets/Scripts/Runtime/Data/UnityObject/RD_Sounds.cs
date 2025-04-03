using System.Collections.Generic;
using Runtime.Data.ValueObject.SoundVOs;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Data.UnityObject
{
    [CreateAssetMenu(menuName = "ArvaveGames/Sound/Sounds", order = 0)]
    public class RD_Sounds : SerializedScriptableObject
    {
        public float maxAudioVolume;
        public SoundData initialSoundSettings;
        public SoundData currentSoundSettings;
        public SoundData savedSoundSettings;
        public Dictionary<GameSounds, AudioClip> Sounds;
    }
}