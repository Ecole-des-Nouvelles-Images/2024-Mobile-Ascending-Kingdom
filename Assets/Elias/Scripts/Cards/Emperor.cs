using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "EmperorCard", menuName = "Cards/Emperor Card")]
    public class Emperor : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Emperor";
            cardDescription = "Represents authority, establishment, and structure.";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                GameManager.Instance.FreezeEveryFifteenBlocks();
            }
        }
    }
}