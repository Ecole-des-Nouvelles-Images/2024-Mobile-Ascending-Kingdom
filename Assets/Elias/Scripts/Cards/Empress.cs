using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "EmpressCard", menuName = "Cards/Empress Card")]
    public class Empress : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Empress";
            cardDescription = "Represents fertility, beauty, and nature.";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                
                
            }
        }
    }
}