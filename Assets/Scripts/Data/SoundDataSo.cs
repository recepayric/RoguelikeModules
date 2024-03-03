using System.Collections.Generic;
using Runtime.Enums;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SoundData", order = 1)]
    public class SoundDataSo : SerializedScriptableObject
    {
        public Dictionary<Sounds, AudioClip> audioClips = new Dictionary<Sounds, AudioClip>();
    }

    public enum Sounds
    {
        None,
        FireMagicCast,
        FireMagicHit,
        ButtonClickMenu,
        ButtonHoverMenu,
    }
}