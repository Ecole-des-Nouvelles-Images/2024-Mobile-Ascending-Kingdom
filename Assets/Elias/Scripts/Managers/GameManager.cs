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

    private float _initialHeightTreshold = 10f;
    private float _initiaScoretreshold = 5f;
    private float _initiaPhaseTreshold = 20f;

    private float _currentHeightTreshold;
    private float _currentScoreTreshold;
    private float _currentPhaseTreshold;

    public GameObject cardPanel;
    private List<CardSO> _deckCards = new List<CardSO>();
    private List<CardSO> _poolCards = new List<CardSO>();
    private List<CardSO> _displayedCards = new List<CardSO>();
    private List<Button> _cardButtons = new List<Button>();

    private List<EventSO> _allEvents;
    private EventSO _currentEvent;

    public List<Sprite> Backgrounds;
    private Sprite _currentBackground;
    private List<Sprite> _backgroundSequence = new List<Sprite>();
    private int _backgroundIndex = 0;

    // Reference to the GameObject whose sprite will be changed
    public GameObject backgroundObject;

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

        // Initialize _poolCards with all available cards
        _poolCards = new List<CardSO>
        {
            ScriptableObject.CreateInstance<Fool>(),
            ScriptableObject.CreateInstance<World>(),
            ScriptableObject.CreateInstance<Devil>(),
            ScriptableObject.CreateInstance<Sun>(),
            ScriptableObject.CreateInstance<Moon>(),
            ScriptableObject.CreateInstance<Death>(),
            ScriptableObject.CreateInstance<Emperor>(),
            ScriptableObject.CreateInstance<Empress>(),
            ScriptableObject.CreateInstance<Tower>(),

        };

        _allEvents = new List<EventSO>
        {
            ScriptableObject.CreateInstance<Wind>(),
            ScriptableObject.CreateInstance<Volcano>(),
            ScriptableObject.CreateInstance<Blizzard>(),
            ScriptableObject.CreateInstance<Wave>(),
        };

        // Initialize _cardButtons with your UI buttons
        _cardButtons = new List<Button>();
        foreach (Transform child in cardPanel.transform)
        {
            Button button = child.GetComponent<Button>();
            if (button != null)
            {
                _cardButtons.Add(button);
            }
        }

        // Hide the card panel initially
        cardPanel.SetActive(false);

        // Initialize background sequence
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

        if (_allBlocCount >= _eventTreshold)
        {
            EventTrigger(_currentEvent);
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
        _currentScoreTreshold = (_currentScoreTreshold + (_initiaScoretreshold * 1.1f));

        Debug.Log("Current score: " + _currentScoreTreshold);
        Debug.Log("Current height: " + _currentHeightTreshold);

        cardPanel.SetActive(true);

        // Pick 3 random cards
        _displayedCards.Clear();
        for (int i = 0; i < 3 && _poolCards.Count > 0; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, _poolCards.Count);
            CardSO pickedCard = _poolCards[randomIndex];
            _displayedCards.Add(pickedCard);
            _poolCards.RemoveAt(randomIndex);
        }

        // Update buttons with picked cards
        for (int i = 0; i < _cardButtons.Count; i++)
        {
            if (i < _displayedCards.Count)
            {
                Button button = _cardButtons[i];
                CardSO card = _displayedCards[i];
                button.image.sprite = card.cardImage;

                // Get all TMP_Text components in the button's children
                TMP_Text[] textComponents = button.GetComponentsInChildren<TMP_Text>();

                // Assign the card name and description to the appropriate TMP_Text components
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
        _deckCards.Add(card);
        card.TriggerEvent();
        _displayedCards.Remove(card);
        button.gameObject.SetActive(false);

        // Return the other two cards to the pool
        foreach (var displayedCard in _displayedCards)
        {
            _poolCards.Add(displayedCard);
        }
        _displayedCards.Clear();

        // Hide the card panel
        cardPanel.SetActive(false);
    }

    public void EventTrigger(EventSO phaseEvent)
    {
        _eventTreshold += 15;
        phaseEvent.TriggerEvent();
    }

    public void PhaseTrigger()
    {
        _currentPhaseTreshold = (_currentPhaseTreshold + (_initiaPhaseTreshold * 1.1f));
        _currentHeightTreshold = (_currentPhaseTreshold + (_currentHeightTreshold));
        _currentScoreTreshold = (_currentPhaseTreshold + (_currentScoreTreshold));

        Debug.Log("new phase treshold" + _currentPhaseTreshold);

        cubeSpawner.FreezeCubes();

        // Change background and update current event
        ChangeBackgroundAndEvent();
    }

    private void InitializeBackgroundSequence()
    {
        int randomIndex = UnityEngine.Random.Range(0, Backgrounds.Count);
        _currentBackground = Backgrounds[randomIndex];
        _backgroundSequence.Add(_currentBackground);
        Backgrounds.RemoveAt(randomIndex);

        while (Backgrounds.Count > 0)
        {
            randomIndex = UnityEngine.Random.Range(0, Backgrounds.Count);
            _backgroundSequence.Add(Backgrounds[randomIndex]);
            Backgrounds.RemoveAt(randomIndex);
        }

        Backgrounds = new List<Sprite>(_backgroundSequence);

        // Set the initial background sprite
        if (backgroundObject != null)
        {
            Image spriteRenderer = backgroundObject.GetComponent<Image>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = _currentBackground;
            }
        }
    }

    private void ChangeBackgroundAndEvent()
    {
        _backgroundIndex = (_backgroundIndex + 1) % _backgroundSequence.Count;
        _currentBackground = _backgroundSequence[_backgroundIndex];

        _currentEvent = _allEvents[_backgroundIndex];

        // Update the background sprite
        if (backgroundObject != null)
        {
            SpriteRenderer spriteRenderer = backgroundObject.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = _currentBackground;
            }
        }
    }
}
}