using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "FoolCard", menuName = "Cards/Fool Card")]
    public class Fool : CardSO
    {
        private void OnEnable()
        {
            cardName = "Le Mat";
            cardDescription = "Détruit trois blocs adjacent de même forme";
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
