using Runtime.Data.UnityObject;
using Runtime.Enums;
using UnityEngine;

namespace Runtime.Model.Interfaces
{
    public interface ISoundModel
    {
        RD_Sounds Sounds { get;  }
        AudioSource AudioSourceMusic { get; set; }
        AudioSource AudioSourceSoundEffects { get; set; }
        void PlaySound(GameSounds sound);
        void PlayMusic(GameSounds music);
    }

    public class SoundModel : ISoundModel
    {
        private RD_Sounds _sounds { get; set; }
        public AudioSource AudioSourceMusic { get; set; }
        public AudioSource AudioSourceSoundEffects { get; set; }

        public void PlaySound(GameSounds sound)
        {
            if (!_sounds.Sounds.ContainsKey(sound))
            {
                //Debug.LogWarning("The sound you are trying to play is not registered to dictionary");
                return;
            }
            
            var soundClip = _sounds.Sounds[sound];
            AudioSourceSoundEffects.PlayOneShot(soundClip);
        }

        public void PlayMusic(GameSounds music)
        {
            if (!_sounds.Sounds.ContainsKey(music))
            {
                //Debug.LogWarning("The music you are trying to play is not registered to dictionary");
                return;
            }
            
            var soundClip = _sounds.Sounds[music];
            AudioSourceMusic.clip = soundClip;
            AudioSourceMusic.Play();
        }

        [PostConstruct]
        public void OnPostConstruct()
        {
            _sounds = Resources.Load<RD_Sounds>("Data/Sounds/Sound");
        }
        
        
        public RD_Sounds Sounds
        {
            get
            {
                if (_sounds == null)
                    OnPostConstruct();
                return _sounds;
            }
        }
        
    }
}