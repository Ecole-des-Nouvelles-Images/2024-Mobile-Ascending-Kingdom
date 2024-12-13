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
            GameManager.Instance.cubeSpawner.isPaused = true;
            menuPanel.SetActive(true);
        }

        public void Resume()
        {
            GameManager.Instance.cubeSpawner.isPaused = false;
            menuPanel.SetActive(false);
        }
        
        public void Restart()
        {
            SceneManager.LoadScene("Elias_Game");
        }
    }
}