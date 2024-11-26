using Elias.Scripts.Managers;
using TMPro;
using UnityEngine;

namespace Elias.Scripts.Scores
{
    public class HeightTracking : MonoBehaviour
    {
        public float Height = 0;
        public TMP_Text HeightText;

        private void Update()
        {
            UpdateHeight();
        }

        private void UpdateHeight()
        {
            float maxHeight = 0;

            for (int i = GameManager.Instance.Blocs.Count - 1; i >= 0; i--)
            {
                var bloc = GameManager.Instance.Blocs[i];

                if (bloc == null)
                {
                    GameManager.Instance.Blocs.RemoveAt(i);
                    continue;
                }

                if (bloc.CompareTag("Solid"))
                {
                    float blocHeight = bloc.transform.position.y;
                    if (blocHeight > maxHeight)
                    {
                        maxHeight = blocHeight;
                    }
                }
            }

            Height = maxHeight;
            HeightText.text = "Height: " + Mathf.FloorToInt(Height).ToString(); // Display as integer
        }


    }
}