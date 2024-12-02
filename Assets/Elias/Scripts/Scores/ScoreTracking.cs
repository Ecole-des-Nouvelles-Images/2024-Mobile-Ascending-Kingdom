using Elias.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Elias.Scripts.Scores
{
    public class ScoreTracking : MonoBehaviour
    {
        public TMP_Text ScoreText;

        private void OnEnable()
        {
            GameManager.Instance.OnScoreUpdated += UpdateScoreText;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnScoreUpdated -= UpdateScoreText;
        }

        private void UpdateScoreText()
        {
            ScoreText.text = "Score: " + GameManager.Instance.Score;
        }
    }
}