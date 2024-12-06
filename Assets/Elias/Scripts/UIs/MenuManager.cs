using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Elias.Scripts.UIs
{
    public class MenuManager : MonoBehaviour
    {
        public GameObject menuPanel;
        public GameObject optionsPanel;
        public GameObject creditsPanel;

        public Slider musicSlider;
        public Slider ambientSlider;
        public Slider vfxSlider;

        private void Start()
        {
            ReturnToMenu();
            LoadVolumeSettings();
        }

        public void StartGame()
        {
            SaveVolumeSettings();
            SceneManager.LoadScene("Elias_Game");
        }

        public void StopGame()
        {
            SceneManager.LoadScene("Elias_Menu");
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void Options()
        {
            menuPanel.SetActive(false);
            optionsPanel.SetActive(true);
            creditsPanel.SetActive(false);
        }

        public void Credits()
        {
            menuPanel.SetActive(false);
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }

        public void ReturnToMenu()
        {
            menuPanel.SetActive(true);
            optionsPanel.SetActive(false);
            creditsPanel.SetActive(false);
        }

        private void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
            PlayerPrefs.SetFloat("AmbientVolume", ambientSlider.value);
            PlayerPrefs.SetFloat("VFXVolume", vfxSlider.value);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            ambientSlider.value = PlayerPrefs.GetFloat("AmbientVolume", 1.0f);
            vfxSlider.value = PlayerPrefs.GetFloat("VFXVolume", 1.0f);
        }
    }
}