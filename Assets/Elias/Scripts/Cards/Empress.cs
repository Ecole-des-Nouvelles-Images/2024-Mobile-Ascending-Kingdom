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
                if (bloc.CompareTag("Solid") && !bloc.IsFrozen)
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
                foreach (Bloc bloc in blocsToFreeze)
                {
                    bloc.IsFrozen = true;
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
