using UnityEngine;

[CreateAssetMenu(fileName = "NewCard", menuName = "Cards/New Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public string cardDescription;
    public Sprite cardImage;
    public GameObject cardPrefab;
    public bool isCardActive;

    public virtual void TriggerEvent()
    {
        // event logic
    }
}