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
            cardName = "The Empress";
            cardDescription = "Represents fertility, beauty, and nature.";
        }

        public override void OnBlocListUpdated()
        {
            if (isCardActive)
            {
                List<Bloc> blocsToFreeze = new List<Bloc>();

                foreach (Bloc bloc in GameManager.Instance.Blocs)
                {
                    if (bloc.shape == "L" || bloc.shape == "La" || bloc.shape == "Z" || bloc.shape == "Za" || bloc.shape == "O" || bloc.shape == "I" || bloc.shape == "T")
                    {
                        blocsToFreeze.Add(bloc);
                    }
                }

                GameManager.Instance.cubeSpawner.FreezeCubes(blocsToFreeze);
                
            }
        }
    }
}