using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "DevilCard", menuName = "Cards/Devil Card")]
    public class Devil : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Devil";
            cardDescription = "Represents bondage, addiction, and materialism.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Devil card
        }
    }
}