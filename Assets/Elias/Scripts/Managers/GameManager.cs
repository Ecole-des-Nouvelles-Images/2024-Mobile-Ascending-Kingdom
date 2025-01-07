using System;
using System.Collections.Generic;
using Elias.Scripts.Cards;
using Elias.Scripts.Event;
using Proto.Script;
using TMPro;
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

        private float _initialHeightTreshold = 8f;
        private float _initiaScoretreshold = 12f;
        private float _initiaPhaseTreshold = 15f;

        private float _currentHeightTreshold;
        private float _currentScoreTreshold;
        private float _currentPhaseTreshold;

        public GameObject cardPanel;
        public GameObject gamePanel;
        
        private List<CardSO> _deckCards = new List<CardSO>();
        private List<CardSO> _poolCards = new List<CardSO>();
        private List<CardSO> _displayedCards = new List<CardSO>();
        private List<Button> _cardButtons = new List<Button>();

        private List<EventSO> _allEvents;
        public EventSO _currentEvent;
        public bool EventActive = false;

        public int BlocksDestroyed;
        public int BlocksChanged;

        public GameObject currentBackgroundObject;
        private ParticleSystem _currentPS;

        public CardSO Fool;
        public CardSO Empress;
        public CardSO Emperor;
        public CardSO Moon;

        public EventSO VolcanoEvent;
        public EventSO TempestEvent;
        public EventSO WindEvent;
        public EventSO BlizzardEvent;

        private CardSO _pendingCard; // Store the card to be added if deck is full
        private bool _isReplacingCard; // Flag to track if we're in replacement mode

        public ParticleSystem VolcanoPS;
        public ParticleSystem TempestPS;
        public ParticleSystem RainPS;
        public ParticleSystem WindPS;
        public ParticleSystem BlizzardPS;

        public ParticleSystem MeteorPS;
        public ParticleSystem LightningPS;

        public MeterorSpawning MeterorSpawningObj;

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


        private void Awake()
        {
            cardPanel = GameObject.FindGameObjectWithTag("CardPanel");
            gamePanel = GameObject.FindGameObjectWithTag("GamePanel");
        }

        private void Start()
        {
            cubeSpawner = FindObjectOfType<CubeSpawner>();
            
            _currentHeightTreshold = _initialHeightTreshold;
            _currentScoreTreshold = _initiaScoretreshold;
            _currentPhaseTreshold = _initiaPhaseTreshold;

            // Add public CardSO instances to the pool
            _poolCards = new List<CardSO>
            {
                Fool,
                Moon,
                Emperor,
                Empress
            };

            _allEvents = new List<EventSO>
            {
                TempestEvent,
                VolcanoEvent,
                WindEvent,
                BlizzardEvent,
                
                
                
            };

            if (_allEvents.Count > 0)
            {
                _currentEvent = _allEvents[0];
            }

            _cardButtons = new List<Button>();
            foreach (Transform child in cardPanel.transform)
            {
                Button button = child.GetComponent<Button>();
                if (button != null)
                {
                    _cardButtons.Add(button);
                }
            }

            cardPanel.SetActive(false);
            gamePanel.SetActive(true);
            InitializeBackgroundSequence();
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

            if (_allBlocCount >= _eventTreshold && !EventActive)
            {
                EventTrigger(_currentEvent);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                TestEvent();
            }
        }

        private void TestEvent()
        {
            _currentEvent.TriggerEvent();
        }

        public void AddBloc(Bloc piece)
        {
            if (piece == null)
            {
                Debug.LogError("Tried to add a null Bloc to Blocs list.");
                return;
            }

            Blocs.Add(piece);
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
            cubeSpawner.SetPauseState(true);
            Debug.Log("Card triggered");
            
            cardPanel.SetActive(true);
            gamePanel.SetActive(false);

            _currentHeightTreshold = (_currentHeightTreshold + (_initialHeightTreshold * 1.1f));
            _currentScoreTreshold = (_currentScoreTreshold + (_initiaScoretreshold * 1.1f));

            Debug.Log("Current score: " + _currentScoreTreshold);
            Debug.Log("Current height: " + _currentHeightTreshold);

            

            _displayedCards.Clear();
            for (int i = 0; i < 3 && _poolCards.Count > 0; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, _poolCards.Count);
                CardSO pickedCard = _poolCards[randomIndex];
                _displayedCards.Add(pickedCard);
                _poolCards.RemoveAt(randomIndex);
            }

            for (int i = 0; i < _cardButtons.Count; i++)
            {
                if (i < _displayedCards.Count)
                {
                    Button button = _cardButtons[i];
                    CardSO card = _displayedCards[i];
                    
                    Image[] imageComponents = button.GetComponentsInChildren<Image>();
                    if (imageComponents.Length >= 2)
                    {
                        imageComponents[1].sprite = card.cardImage;
                    }

                    TMP_Text[] textComponents = button.GetComponentsInChildren<TMP_Text>();

                    if (textComponents.Length >= 2)
                    {
                        textComponents[0].text = card.cardName;
                        textComponents[1].text = card.cardDescription;
                    }

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
            if (_deckCards.Count >= 3)
            {
                _pendingCard = card;
                _isReplacingCard = true;
                DisplayDeckCardsForReplacement();
            }
            else
            {
                AddCardToDeck(card);
            }
        }

        private void DisplayDeckCardsForReplacement()
        {
            _displayedCards.Clear();
            _displayedCards.AddRange(_deckCards);

            for (int i = 0; i < _cardButtons.Count; i++)
            {
                if (i < _displayedCards.Count)
                {
                    Button button = _cardButtons[i];
                    CardSO card = _displayedCards[i];
                    button.image.sprite = card.cardImage;

                    TMP_Text[] textComponents = button.GetComponentsInChildren<TMP_Text>();

                    if (textComponents.Length >= 2)
                    {
                        textComponents[0].text = card.cardName;
                        textComponents[1].text = card.cardDescription;
                    }

                    button.onClick.RemoveAllListeners();
                    button.onClick.AddListener(() => OnReplaceCardButtonClick(button, card));
                    button.gameObject.SetActive(true);
                }
                else
                {
                    _cardButtons[i].gameObject.SetActive(false);
                }
            }
        }

        private void OnReplaceCardButtonClick(Button button, CardSO cardToRemove)
        {
            RemoveCardFromDeck(cardToRemove);
            AddCardToDeck(_pendingCard);
            _pendingCard = null;
            _isReplacingCard = false;
            cubeSpawner.SetPauseState(false);
            cardPanel.SetActive(false);
            gamePanel.SetActive(true);
        }

        private void AddCardToDeck(CardSO card)
        {
            _deckCards.Add(card);
            card.isCardActive = true;
            card.Register();
            _displayedCards.Remove(card);

            foreach (var displayedCard in _displayedCards)
            {
                _poolCards.Add(displayedCard);
            }
            _displayedCards.Clear();

            cubeSpawner.SetPauseState(false);
            cardPanel.SetActive(false);
            gamePanel.SetActive(true);
        }

        public void RemoveCardFromDeck(CardSO card)
        {
            if (_deckCards.Contains(card))
            {
                card.isCardActive = false;
                card.Unregister(); // Unregister the card
                _deckCards.Remove(card);
            }
        }

        public void EventTrigger(EventSO phaseEvent)
        {
            if (!EventActive)
            {
                phaseEvent.TriggerEvent();
            }
        }

        public void PhaseTrigger()
        {
            _currentPhaseTreshold = (_currentPhaseTreshold + (_initiaPhaseTreshold * 1.1f));
            _currentHeightTreshold = (_currentPhaseTreshold + (_currentHeightTreshold));
            _currentScoreTreshold = (_currentPhaseTreshold + (_currentScoreTreshold));

            Debug.Log("new phase treshold" + _currentPhaseTreshold);

            // Create a new list that excludes the most recently added block
            var blocksToFreeze = new List<Bloc>(Blocs);
            if (blocksToFreeze.Count > 1)
            {
                blocksToFreeze.RemoveAt(blocksToFreeze.Count - 1); // Remove the last block
            }

            cubeSpawner.FreezeCubes(blocksToFreeze);

            ChangeBackgroundAndEvent();
        }

        
        private void InitializeBackgroundSequence()
        {
            if (_currentEvent != null)
            {
                UpdateBackgroundAndParticleSystem(_currentEvent);
            }
        }

        private void ChangeBackgroundAndEvent()
        {
            int nextEventIndex = (_allEvents.IndexOf(_currentEvent) + 1) % _allEvents.Count;
            _currentEvent = _allEvents[nextEventIndex];
            UpdateBackgroundAndParticleSystem(_currentEvent);
        }

        private void UpdateBackgroundAndParticleSystem(EventSO eventSO)
        {
            if (currentBackgroundObject != null)
            {
                Image spriteRenderer = currentBackgroundObject.GetComponent<Image>();
                if (spriteRenderer != null)
                {
                    spriteRenderer.sprite = eventSO.eventBackground;
                }
            }

            if (_currentPS != null)
            {
                Destroy(_currentPS.gameObject);
            }
        }

        public void ResetEventThreshold()
        {
            _eventTreshold += 15;
        }
    }
}
