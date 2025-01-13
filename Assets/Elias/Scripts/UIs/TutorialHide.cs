using System;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.UIs
{
    public class TutorialHide : MonoBehaviour
    {
        private void Update()
        {
            if (GameManager.Instance.Score == 3)
            {
                this.gameObject.SetActive(false);
            }
        }
    }
}
