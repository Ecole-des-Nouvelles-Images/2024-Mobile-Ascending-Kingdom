using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    public class CardSO : ScriptableObject
    {
        public string cardName;
        public string cardDescription;
        public Sprite cardImage;
        public bool isCardActive;

        public void Register()
        {
            if (isCardActive)
            {
                GameManager.Instance.OnBlocListUpdated += OnBlocListUpdated;
            }
        }

        public void Unregister()
        {
            GameManager.Instance.OnBlocListUpdated -= OnBlocListUpdated;
        }

        public virtual void OnBlocListUpdated()
        {
            // To be overridden with specific card logic.
        }
    }
}