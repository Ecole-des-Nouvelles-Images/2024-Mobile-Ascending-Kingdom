using System;
using System.Collections.Generic;
using Elias.Scripts.Event;
using Proto.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Elias.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        private CubeSpawner _cubeSpawner;
        
        public List<Bloc> Blocs = new List<Bloc>();
        public UnityAction OnBlocListUpdated;

        private float _height = 0;
        private int _score = 0;
        
        private int _allBlocCount = 0;
        private int _eventTreshold = 15;

        private float _initialHeightTreshold = 10f;
        private float _initiaScoretreshold = 20f;
        private float _initiaPhaseTreshold = 20f;
        
        private float _currentHeightTreshold;
        private float _currentScoreTreshold;
        private float _currentPhaseTreshold;
        
        private EventSO _activeEvent;

        public List<Sprite> Backgrounds;
        private Sprite _currentBackground;

        public float Height { 
            get => _height;
            private set { _height = value; 
                OnHeightUpdated?.Invoke(); } 
        }
        public int Score { 
            get => _score;
            private set { _score = value;
                OnScoreUpdated?.Invoke(); } 
        }

        public UnityAction OnHeightUpdated;
        public UnityAction OnScoreUpdated;

        private void Start()
        {
            _currentHeightTreshold = _initialHeightTreshold;
            _currentScoreTreshold = _initiaScoretreshold;
            _currentPhaseTreshold = _initiaPhaseTreshold;
            
            _currentBackground = Backgrounds[0];
        }

        private void Update()
        {
            if (_height >= _currentHeightTreshold || _score >= _currentScoreTreshold)
            {
                _currentHeightTreshold = (_currentHeightTreshold + (_initialHeightTreshold * 0.1f));
                _currentScoreTreshold *= (_currentScoreTreshold + (_initiaScoretreshold * 0.1f));
                CardTrigger();
            }

            if (_height >= _currentPhaseTreshold)
            {
                _currentPhaseTreshold = (_currentPhaseTreshold + (_initiaPhaseTreshold * 0.1f));
                _currentHeightTreshold = (_currentPhaseTreshold + (_currentHeightTreshold));
                _currentScoreTreshold *= (_currentPhaseTreshold + (_currentScoreTreshold));
                PhaseTrigger();
            }

            if (_allBlocCount >= _eventTreshold)
            {
                _eventTreshold += 15;
                EventTrigger(_activeEvent);
            }
        }

        public void AddBloc(Bloc piece)
        {
            if (piece == null)
            {
                Debug.LogError("Tried to add a null Bloc to Blocs list.");
                return;
            }

            Blocs.Add(piece);
            Debug.Log($"Added Bloc {piece.name} to Blocs list. Total count: {Blocs.Count}");
            UpdateHeightAndScore();
            OnBlocListUpdated?.Invoke();
            _allBlocCount++;
        }

        public void RemoveBloc(Bloc piece)
        {
            if (piece != null && Blocs.Contains(piece))
            {
                Blocs.Remove(piece);
                UpdateHeightAndScore();
                OnBlocListUpdated?.Invoke();
            }
        }

        public int GetBlocCount()
        {
            return Blocs.Count;
        }

        private void UpdateHeightAndScore()
        {
            float maxHeight = 0;
            int blocCount = 0;

            for (int i = Blocs.Count - 1; i >= 0; i--)
            {
                var bloc = Blocs[i];

                if (bloc == null)
                {
                    Blocs.RemoveAt(i);
                    continue;
                }

                if (bloc.CompareTag("Solid"))
                {
                    float blocHeight = bloc.transform.position.y;
                    if (blocHeight > maxHeight)
                    {
                        maxHeight = blocHeight;
                    }
                }

                blocCount++;
            }

            Height = maxHeight;
            Score = blocCount;
        }

        public void CardTrigger() {
            Debug.Log("Card triggered");
        }

        public void EventTrigger(EventSO eventSO) {
            Debug.Log("Event triggered");
        }
        
        public void PhaseTrigger()
        {
            _cubeSpawner.FreezeCubes();
        }
        
        
    }
}
