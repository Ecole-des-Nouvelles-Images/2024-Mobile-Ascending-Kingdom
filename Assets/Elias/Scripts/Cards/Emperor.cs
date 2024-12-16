using Elias.Scripts.Managers;
using UnityEngine;

/*namespace Elias.Scripts.Cards
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
}*/

using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "EmperorCard", menuName = "Cards/Emperor Card")]
    public class Emperor : CardSO
    {
        private void OnEnable()
        {
            cardName = "L'Empereur";
            cardDescription = "Fait pousser des lierres toutes les 20 pièces.";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                FreezeAdjacentBlocksEveryTwenty();
            }
        }

        private void FreezeAdjacentBlocksEveryTwenty()
        {
            int totalBlocs = GameManager.Instance.GetBlocCount();
            if (totalBlocs > 0 && totalBlocs % 20 == 0)
            {
                Bloc lastBloc = GameManager.Instance.Blocs[^1];
                if (lastBloc != null && lastBloc.CompareTag("Solid"))
                {
                    List<Bloc> adjacentBlocs = GetAdjacentSolidBlocs(lastBloc);
                    adjacentBlocs.Add(lastBloc);
                    GameManager.Instance.cubeSpawner.FreezeCubes(adjacentBlocs);
                }
            }
        }

        private List<Bloc> GetAdjacentSolidBlocs(Bloc bloc)
        {
            List<Bloc> adjacentBlocs = new List<Bloc>();
            Vector3 blocPosition = bloc.transform.position;

            foreach (Bloc otherBloc in GameManager.Instance.Blocs)
            {
                if (otherBloc != bloc && otherBloc.CompareTag("Solid"))
                {
                    Vector3 otherBlocPosition = otherBloc.transform.position;
                    if (Vector3.Distance(blocPosition, otherBlocPosition) <= 1.5f) // Adjust the distance threshold as needed
                    {
                        adjacentBlocs.Add(otherBloc);
                    }
                }
            }

            return adjacentBlocs;
        }
    }
}
