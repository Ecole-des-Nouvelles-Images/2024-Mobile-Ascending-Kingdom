using UnityEngine;

[CreateAssetMenu(fileName = "FoolCard", menuName = "Cards/Fool Card")]
public class Fool : CardSO
{
    public override void TriggerEvent()
    {
        Debug.Log("Fool Card Triggered!");
    }
}

[CreateAssetMenu(fileName = "WorldCard", menuName = "Cards/World Card")]
public class World : CardSO
{
    public override void TriggerEvent()
    {
        Debug.Log("World Card Triggered!");
    }
}

[CreateAssetMenu(fileName = "DevilCard", menuName = "Cards/Devil Card")]
public class Devil : CardSO
{
    public override void TriggerEvent()
    {
        Debug.Log("Devil Card Triggered!");
    }
}