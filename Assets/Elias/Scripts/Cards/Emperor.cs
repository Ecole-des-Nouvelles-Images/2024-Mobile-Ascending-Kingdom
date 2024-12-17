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
            cardDescription = "Fait pousser des lierres toutes les 15 pièces.";
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
            Debug.Log(totalBlocs);
            if (totalBlocs > 0 && totalBlocs % 15 == 0)
            {
                Bloc lastBloc = GameManager.Instance.Blocs[^2];
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

            // Use Physics.OverlapSphere to detect nearby colliders
            Collider[] colliders = Physics.OverlapSphere(blocPosition, 1.5f);

            foreach (Collider collider in colliders)
            {
                Bloc otherBloc = collider.GetComponent<Bloc>();
                if (otherBloc != null && otherBloc != bloc && otherBloc.CompareTag("Solid"))
                {
                    adjacentBlocs.Add(otherBloc);
                }
            }

            return adjacentBlocs;
        }
    }
}
