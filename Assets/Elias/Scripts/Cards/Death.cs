using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "DeathCard", menuName = "Cards/Death Card")]
    public class Death : CardSO
    {
        private void OnEnable()
        {
            cardName = "Death";
            cardDescription = "Represents endings, transitions, and change.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Death card
        }
    }
}