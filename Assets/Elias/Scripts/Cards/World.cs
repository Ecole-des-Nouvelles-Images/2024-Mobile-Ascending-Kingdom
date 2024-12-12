using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "WorldCard", menuName = "Cards/World Card")]
    public class World : CardSO
    {
        private void OnEnable()
        {
            cardName = "The World";
            cardDescription = "Represents completion, accomplishment, and travel.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for World card
        }
    }
}