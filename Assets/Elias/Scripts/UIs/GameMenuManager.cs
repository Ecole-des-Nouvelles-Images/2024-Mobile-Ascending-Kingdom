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
            GameManager.Instance.cubeSpawner.isPaused = true;
        }

        public void Resume()
        {
            menuPanel.SetActive(false);
            
            GameManager.Instance.cubeSpawner.isPaused = false;
        }
        
        public void Restart()
        {
            SceneManager.LoadScene("Elias_Game");
        }
    }
}