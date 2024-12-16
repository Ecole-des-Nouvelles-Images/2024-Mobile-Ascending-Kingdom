using System.Collections.Generic;
using Elias.Scripts.Managers;
using UnityEngine;

namespace Elias.Scripts.Cards
{
    [CreateAssetMenu(fileName = "MoonCard", menuName = "Cards/Moon Card")]
    public class Moon : CardSO
    {
        private void OnEnable()
        {
            cardName = "La Lune";
            cardDescription = "Solidifie 6 blocs adjacent de couleurs différentes.";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                FreezeAdjacentDifferentColorBlocks();
            }
        }

        private void FreezeAdjacentDifferentColorBlocks()
        {
            List<Bloc> blocsToFreeze = new List<Bloc>();

            foreach (Bloc bloc in GameManager.Instance.Blocs)
            {
                if (bloc.CompareTag("Solid"))
                {
                    List<Bloc> adjacentBlocs = GetAdjacentSolidBlocs(bloc);
                    if (adjacentBlocs.Count >= 5)
                    {
                        HashSet<Color> uniqueColors = new HashSet<Color>
                        {
                            bloc.GetComponent<MeshRenderer>().material.color
                        };

                        bool allDifferentColors = true;

                        foreach (Bloc adjacentBloc in adjacentBlocs)
                        {
                            if (!uniqueColors.Add(adjacentBloc.GetComponent<MeshRenderer>().material.color))
                            {
                                allDifferentColors = false;
                                break;
                            }
                        }

                        if (allDifferentColors)
                        {
                            blocsToFreeze.Add(bloc);
                            blocsToFreeze.AddRange(adjacentBlocs);
                        }
                    }
                }
            }

            if (blocsToFreeze.Count >= 6)
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
