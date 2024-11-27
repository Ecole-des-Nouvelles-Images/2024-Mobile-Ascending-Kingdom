using Elias.Scripts.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Elias.Scripts.Scores
{
    public class ScoreTracking : MonoBehaviour
    {
        public int Score = 0;
        public TMP_Text ScoreText;

        private void OnEnable()
        {
            GameManager.Instance.OnBlocListUpdated += UpdateScore;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnBlocListUpdated -= UpdateScore;
        }

        private void UpdateScore()
        {
            Score = GameManager.Instance.GetBlocCount();
            ScoreText.text = "Score: " + Score;
        }
    }
}

