using System;
using System.Collections.Generic;
using Elias.Scripts.Event;
using Proto.Script;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Elias.Scripts.Managers
{
    public class GameManager : MonoBehaviourSingleton<GameManager>
    {
        public CubeSpawner cubeSpawner;

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

        public GameObject cardPanel;
        private List<CardSO> _activeCards = new List<CardSO>();
        private List<CardSO> _optionsCards = new List<CardSO>();
        private List<CardSO> _displayedCards = new List<CardSO>();
        private List<Button> _cardButtons = new List<Button>();

        private EventSO _activeEvent;

        public List<Sprite> Backgrounds;
        private Sprite _currentBackground;

        public float Height
        {
            get => _height;
            private set
            {
                _height = value;
                OnHeightUpdated?.Invoke();
            }
        }
        public int Score
        {
            get => _score;
            private set
            {
                _score = value;
                OnScoreUpdated?.Invoke();
            }
        }

        public UnityAction OnHeightUpdated;
        public UnityAction OnScoreUpdated;

        private void Start()
        {
            _currentHeightTreshold = _initialHeightTreshold;
            _currentScoreTreshold = _initiaScoretreshold;
            _currentPhaseTreshold = _initiaPhaseTreshold;

            // Initialize _optionsCards with all available cards
            _optionsCards = new List<CardSO> { /* Add your cards here */ };

            // Initialize _cardButtons with your UI buttons
            _cardButtons = new List<Button> { /* Add your buttons here */ };
        }

        private void Update()
        {
            if (_height >= _currentHeightTreshold || _score >= _currentScoreTreshold)
            {
                CardTrigger();
            }

            if (_height >= _currentPhaseTreshold)
            {
                PhaseTrigger();
            }

            if (_allBlocCount >= _eventTreshold)
            {
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

        public void CardTrigger()
        {
            Debug.Log("Card triggered");

            _currentHeightTreshold = (_currentHeightTreshold + (_initialHeightTreshold * 1.1f));
            _currentScoreTreshold *= (_currentScoreTreshold + (_initiaScoretreshold * 1.1f));

            cardPanel.SetActive(true);

            // Pick 3 random cards
            _displayedCards.Clear();
            for (int i = 0; i < 3 && _optionsCards.Count > 0; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, _optionsCards.Count);
                CardSO pickedCard = _optionsCards[randomIndex];
                _displayedCards.Add(pickedCard);
                _optionsCards.RemoveAt(randomIndex);
            }

            // Update buttons with picked cards
            for (int i = 0; i < _cardButtons.Count; i++)
            {
                if (i < _displayedCards.Count)
                {
                    Button button = _cardButtons[i];
                    CardSO card = _displayedCards[i];
                    button.image.sprite = card.cardImage;
                    button.GetComponentInChildren<Text>().text = card.cardName;
                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => OnCardButtonClick(button, card));
                    button.gameObject.SetActive(true);
                }
                else
                {
                    _cardButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnCardButtonClick(Button button, CardSO card)
        {
            _activeCards.Add(card);
            card.TriggerEvent();
            _displayedCards.Remove(card);
            button.gameObject.SetActive(false);

            // Return the other two cards to the pool
            foreach (var displayedCard in _displayedCards)
            {
                _optionsCards.Add(displayedCard);
            }
            _displayedCards.Clear();

            // Hide the card panel
            cardPanel.SetActive(false);
        }

        public void EventTrigger(EventSO eventSO)
        {
            _eventTreshold += 15;
            Debug.Log("Event triggered");
        }

        public void PhaseTrigger()
        {
            _currentPhaseTreshold = (_currentPhaseTreshold + (_initiaPhaseTreshold * 1.1f));
            _currentHeightTreshold = (_currentPhaseTreshold + (_currentHeightTreshold));
            _currentScoreTreshold *= (_currentPhaseTreshold + (_currentScoreTreshold));

            Debug.Log("new phase treshold" + _currentPhaseTreshold);

            cubeSpawner.FreezeCubes();
        }
    }
}
