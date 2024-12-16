using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "EmpressCard", menuName = "Cards/Empress Card")]
    public class Empress : CardSO
    {
        private void OnEnable()
        {
            cardName = "L'Impératrice";
            cardDescription = "Solidifie 3 blocs adjacents de même couleurs";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                FreezeAdjacentSameColorBlocks();
            }
        }

        private void FreezeAdjacentSameColorBlocks()
        {
            List<Bloc> blocsToFreeze = new List<Bloc>();

            foreach (Bloc bloc in GameManager.Instance.Blocs)
            {
                if (bloc.CompareTag("Solid"))
                {
                    List<Bloc> adjacentBlocs = GetAdjacentSolidBlocs(bloc);
                    if (adjacentBlocs.Count >= 2)
                    {
                        bool allSameColor = true;
                        Color blocColor = bloc.GetComponent<MeshRenderer>().material.color;

                        foreach (Bloc adjacentBloc in adjacentBlocs)
                        {
                            if (adjacentBloc.GetComponent<MeshRenderer>().material.color != blocColor)
                            {
                                allSameColor = false;
                                break;
                            }
                        }

                        if (allSameColor)
                        {
                            blocsToFreeze.Add(bloc);
                            blocsToFreeze.AddRange(adjacentBlocs);
                        }
                    }
                }
            }

            if (blocsToFreeze.Count >= 3)
            {
                GameManager.Instance.cubeSpawner.FreezeCubes(blocsToFreeze);
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
