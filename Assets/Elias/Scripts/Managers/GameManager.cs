using System.Collections.Generic;
using Proto.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elias.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public List<Cube> Blocs = new List<Cube>();

        public UnityAction OnBlocListUpdated;

        public void AddBloc(Cube bloc)
        {
            Blocs.Add(bloc);
            OnBlocListUpdated?.Invoke();
        }
        
        public void RemoveBloc(Cube bloc)
        {
            if (bloc != null && Blocs.Contains(bloc))
            {
                Blocs.Remove(bloc);
                OnBlocListUpdated?.Invoke();
            }
        }

        
        public int GetBlocCount()
        {
            return Blocs.Count;
        }
    }
}