using System;
using Data;
using UnityEngine;

namespace Runtime.Managers
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public SoundDataSo soundDataSo;
        
        public void PlayOnce(Sounds sound, float volume)
        {
            volume = 0.5f;
            if (sound == Sounds.None) return;
            audioSource.PlayOneShot(soundDataSo.audioClips[sound], volume);
        }
        
        private void Start()
        {
            EventManager.Instance.PlaySoundOnceEvent += PlayOnce;
        }

        private void OnDestroy()
        {
            EventManager.Instance.PlaySoundOnceEvent -= PlayOnce;
        }
    }
}