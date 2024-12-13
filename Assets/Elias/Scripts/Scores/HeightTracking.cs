using Elias.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Elias.Scripts.Scores
{
    public class HeightTracking : MonoBehaviour
    {
        public TMP_Text HeightText;

        private void OnEnable()
        {
            GameManager.Instance.OnHeightUpdated += UpdateHeightText;
        }

        private void OnDisable()
        {
            GameManager.Instance.OnHeightUpdated -= UpdateHeightText;
        }

        private void UpdateHeightText()
        {
            HeightText.text = Mathf.FloorToInt(GameManager.Instance.Height).ToString() + " m";
        }
    }
}