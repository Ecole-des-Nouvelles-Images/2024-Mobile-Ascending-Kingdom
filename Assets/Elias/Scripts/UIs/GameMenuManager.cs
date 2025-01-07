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

        private void Start()
        {
            menuPanel.SetActive(false);
        }


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

    }
}