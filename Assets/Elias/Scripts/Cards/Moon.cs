using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "MoonCard", menuName = "Cards/Moon Card")]
    public class Moon : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Moon";
            cardDescription = "Represents intuition, dreams, and the subconscious.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Moon card
        }
    }
}