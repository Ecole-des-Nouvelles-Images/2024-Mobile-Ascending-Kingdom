using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.UIs
{
    public class AudioManager : MonoBehaviourSingleton<AudioManager>
    {
        public AudioSource musicSource;
        public AudioSource ambientSource;
        public AudioSource vfxSource;

        public AudioClip mainMenuMusic;
        public AudioClip gameMusic;

        public AudioClip blocPlaced;

        private void Start()
        {
            ApplyVolumeSettings();
        }

        private void ApplyVolumeSettings()
        {
            musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            ambientSource.volume = PlayerPrefs.GetFloat("AmbientVolume", 1.0f);
            vfxSource.volume = PlayerPrefs.GetFloat("VFXVolume", 1.0f);
        }

        public void PlaySound( AudioClip clip )
        {
            vfxSource.PlayOneShot(clip);
        }
    }
}