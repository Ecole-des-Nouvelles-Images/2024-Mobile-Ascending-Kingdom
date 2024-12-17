using System.Collections.Generic;
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
            cardDescription = "Détruit trois blocs adjacents de même forme";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                DestroyAdjacentSameShapeBlocks();
            }
        }

        private void DestroyAdjacentSameShapeBlocks()
        {
            List<Bloc> blocsToDestroy = new List<Bloc>();

            foreach (Bloc bloc in GameManager.Instance.Blocs)
            {
                if (bloc.CompareTag("Solid") && !bloc.IsFrozen)
                {
                    List<Bloc> adjacentBlocs = GetAdjacentSolidBlocs(bloc);
                    if (adjacentBlocs.Count >= 2)
                    {
                        bool allSameShape = true;
                        string blocShape = bloc.Shape;

                        foreach (Bloc adjacentBloc in adjacentBlocs)
                        {
                            if (adjacentBloc.Shape != blocShape)
                            {
                                allSameShape = false;
                                break;
                            }
                        }

                        if (allSameShape)
                        {
                            blocsToDestroy.Add(bloc);
                            blocsToDestroy.AddRange(adjacentBlocs);
                        }
                    }
                }
            }

            if (blocsToDestroy.Count >= 3)
            {
                foreach (Bloc bloc in blocsToDestroy)
                {
                    Destroy(bloc.gameObject);
                    GameManager.Instance.RemoveBloc(bloc);
                }
            }
        }

        private List<Bloc> GetAdjacentSolidBlocs(Bloc bloc)
        {
            List<Bloc> adjacentBlocs = new List<Bloc>();
            Vector3 blocPosition = bloc.transform.position;

            Collider[] colliders = Physics.OverlapSphere(blocPosition, 1.5f);

            foreach (Collider collider in colliders)
            {
                Bloc otherBloc = collider.GetComponent<Bloc>();
                if (otherBloc != null && otherBloc != bloc && otherBloc.CompareTag("Solid") && !otherBloc.IsFrozen)
                {
                    adjacentBlocs.Add(otherBloc);
                }
            }

            return adjacentBlocs;
        }
    }
}
