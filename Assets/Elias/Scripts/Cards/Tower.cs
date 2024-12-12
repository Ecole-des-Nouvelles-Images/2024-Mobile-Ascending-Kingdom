using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "TowerCard", menuName = "Cards/Tower Card")]
    public class Tower : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Tower";
            cardDescription = "Represents sudden change, upheaval, and chaos.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Tower card
        }
    }
}