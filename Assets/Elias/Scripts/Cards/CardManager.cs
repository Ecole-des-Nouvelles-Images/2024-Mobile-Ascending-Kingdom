using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Elias.Scripts.Cards
{
    public class CardManager : MonoBehaviour
    {
        public GameObject CardSelectionPanel;
        public GameObject CardPrefab; // Prefab for displaying card information
        public Transform CardContainer; // Container for holding card UI elements

        public int MaxDeckSize = 3;
        private List<CardSO> _playerDeck = new List<CardSO>();
        private List<CardSO> _currentChoices = new List<CardSO>();

        private void Start()
        {
            CardSelectionPanel.SetActive(false);
        }

        public void OpenCardSelection(List<CardSO> availableCards)
        {
            _currentChoices.Clear();
            foreach (Transform child in CardContainer)
            {
                Destroy(child.gameObject);
            }

            for (int i = 0; i < availableCards.Count; i++)
            {
                _currentChoices.Add(availableCards[i]);
                GameObject cardUI = Instantiate(CardPrefab, CardContainer);
                cardUI.GetComponentInChildren<Text>().text = availableCards[i].cardName;
                cardUI.GetComponent<Button>().onClick.AddListener(() => SelectCard(i));
            }

            CardSelectionPanel.SetActive(true);
        }

        public void SelectCard(int cardIndex)
        {
            if (cardIndex < 0 || cardIndex >= _currentChoices.Count) return;

            CardSO chosenCard = _currentChoices[cardIndex];
            Debug.Log($"Player chose: {chosenCard.cardName}");

            if (_playerDeck.Count < MaxDeckSize)
            {
                _playerDeck.Add(chosenCard);
                Debug.Log($"Card added to deck: {chosenCard.cardName}. Current deck: {string.Join(", ", _playerDeck)}");
            }
            else
            {
                Debug.Log("Deck is full! Cannot add more cards.");
            }

            CardSelectionPanel.SetActive(false);

            ApplyCardEffect(chosenCard);
        }

        private void ApplyCardEffect(CardSO card)
        {
            Debug.Log($"Applying effect for card: {card.cardName}");
            card.TriggerEvent();
        }
    }
}
