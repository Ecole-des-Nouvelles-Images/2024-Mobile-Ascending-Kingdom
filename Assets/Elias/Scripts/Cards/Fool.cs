using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{

    [CreateAssetMenu(fileName = "FoolCard", menuName = "Cards/Fool Card")]
    public class Fool : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Fool";
            cardDescription = "Represents new beginnings, faith, and spontaneity.";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                GameManager.Instance.DestroyStackedIdenticalBlocks();
            }
        }
    }
}
