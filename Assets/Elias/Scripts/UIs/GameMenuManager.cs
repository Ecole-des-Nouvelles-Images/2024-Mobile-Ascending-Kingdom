using System;
using Elias.Scripts.Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Elias.Scripts.UIs
{
    public class GameMenuManager : MonoBehaviour
    {
        public GameObject menuPanel;


        public void StopGame()
        {
            SceneManager.LoadScene("Elias_Menu");
        }
        public void GameMenu()
        {
            menuPanel.SetActive(true);
            GameManager.Instance.cubeSpawner.SetPauseState(true);
        }

        public void Resume()
        {
            menuPanel.SetActive(false);
            GameManager.Instance.cubeSpawner.SetPauseState(false);
        }

        
        public void Restart()
        {
            SceneManager.LoadScene("Elias_Game");
        }
        
        private bool pauseCooldown = false;

        public void TogglePause()
        {
            if (pauseCooldown) return;

            GameManager.Instance.cubeSpawner.SetPauseState(!GameManager.Instance.cubeSpawner.isPaused);
            pauseCooldown = true;
            Invoke(nameof(ResetPauseCooldown), 0.3f); // 300ms delay
        }

        private void ResetPauseCooldown()
        {
            pauseCooldown = false;
        }

    }
}