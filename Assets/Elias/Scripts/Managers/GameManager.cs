using System.Collections.Generic;
using Proto.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elias.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public List<Bloc> Blocs = new List<Bloc>();

        public UnityAction OnBlocListUpdated;

        public void AddBloc(Bloc piece)
        {
            if (piece == null)
            {
                Debug.LogError("Tried to add a null Bloc to Blocs list.");
                return;
            }

            Blocs.Add(piece);
            Debug.Log($"Added Bloc {piece.name} to Blocs list. Total count: {Blocs.Count}");
            OnBlocListUpdated?.Invoke();
        }



        public void RemoveBloc(Bloc piece)
        {
            if (piece != null && Blocs.Contains(piece))
            {
                Blocs.Remove(piece);
                OnBlocListUpdated?.Invoke();
            }
        }

        public int GetBlocCount()
        {
            return Blocs.Count;
        }
    }
}