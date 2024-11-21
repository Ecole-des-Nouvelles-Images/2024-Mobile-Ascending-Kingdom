using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PreProdElias.Scripts.Cards
{
    public class CardManager : MonoBehaviour
    {
        public GameObject CardSelectionPanel;
        public List<Button> CardButtons;
        public Text[] CardTexts;

        public int MaxDeckSize = 3;
        private List<string> _playerDeck = new List<string>();
        private List<string> _currentChoices = new List<string>();

        private void Start()
        {
            CardSelectionPanel.SetActive(false);
        }

        public void OpenCardSelection(List<string> availableCards)
        {
            _currentChoices.Clear();
            for (int i = 0; i < CardButtons.Count; i++)
            {
                if (i < availableCards.Count)
                {
                    _currentChoices.Add(availableCards[i]);
                    CardTexts[i].text = availableCards[i];
                    CardButtons[i].gameObject.SetActive(true);
                }
                else
                {
                    CardButtons[i].gameObject.SetActive(false);
                }
            }

            CardSelectionPanel.SetActive(true);
        }
        public void SelectCard(int cardIndex)
        {
            if (cardIndex < 0 || cardIndex >= _currentChoices.Count) return;

            string chosenCard = _currentChoices[cardIndex];
            Debug.Log($"Player chose: {chosenCard}");

            if (_playerDeck.Count < MaxDeckSize)
            {
                _playerDeck.Add(chosenCard);
                Debug.Log($"Card added to deck: {chosenCard}. Current deck: {string.Join(", ", _playerDeck)}");
            }
            else
            {
                Debug.Log("Deck is full! Cannot add more cards.");
            }

            CardSelectionPanel.SetActive(false);

            ApplyCardEffect(chosenCard);
        }

        private void ApplyCardEffect(string card)
        {
            Debug.Log($"Applying effect for card: {card}");
        }
    }
}
