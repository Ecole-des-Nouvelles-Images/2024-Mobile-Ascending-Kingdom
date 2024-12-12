using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "SunCard", menuName = "Cards/Sun Card")]
    public class Sun : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Sun";
            cardDescription = "Represents positivity, success, and radiance.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Sun card
        }
    }
}