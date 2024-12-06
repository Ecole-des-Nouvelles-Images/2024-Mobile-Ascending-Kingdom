using UnityEngine;

namespace Elias.Scripts.UIs
{
    public class AudioManager : MonoBehaviour
    {
        public AudioSource musicSource;
        public AudioSource ambientSource;
        public AudioSource vfxSource;

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
    }
}