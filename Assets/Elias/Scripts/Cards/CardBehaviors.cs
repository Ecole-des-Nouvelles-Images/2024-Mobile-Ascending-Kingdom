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

    [CreateAssetMenu(fileName = "DeathCard", menuName = "Cards/Death Card")]
    public class Death : CardSO
    {
        private void OnEnable()
        {
            cardName = "Death";
            cardDescription = "Represents endings, transitions, and change.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Death card
        }
    }

    [CreateAssetMenu(fileName = "DevilCard", menuName = "Cards/Devil Card")]
    public class Devil : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Devil";
            cardDescription = "Represents bondage, addiction, and materialism.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Devil card
        }
    }

    [CreateAssetMenu(fileName = "TowerCard", menuName = "Cards/Tower Card")]
    public class Tower : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Tower";
            cardDescription = "Represents sudden change, upheaval, and chaos.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Tower card
        }
    }

    [CreateAssetMenu(fileName = "MoonCard", menuName = "Cards/Moon Card")]
    public class Moon : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Moon";
            cardDescription = "Represents intuition, dreams, and the subconscious.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Moon card
        }
    }

    [CreateAssetMenu(fileName = "SunCard", menuName = "Cards/Sun Card")]
    public class Sun : CardSO
    {
        private void OnEnable()
        {
            cardName = "The Sun";
            cardDescription = "Represents positivity, success, and radiance.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for Sun card
        }
    }

    [CreateAssetMenu(fileName = "WorldCard", menuName = "Cards/World Card")]
    public class World : CardSO
    {
        private void OnEnable()
        {
            cardName = "The World";
            cardDescription = "Represents completion, accomplishment, and travel.";
        }

        public override void OnBlocListUpdated()
        {
            // Placeholder logic for World card
        }
    }
}
