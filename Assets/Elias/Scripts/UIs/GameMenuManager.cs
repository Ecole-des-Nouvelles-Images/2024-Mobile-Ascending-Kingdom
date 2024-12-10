using System;
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
        }

        public void Resume()
        {
            menuPanel.SetActive(false);
        }
    }
}